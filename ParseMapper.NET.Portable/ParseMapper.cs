using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Parse;

namespace ParseMapperNET.Portable
{
    public class ParseMapper : IParseMapper
    {
        #region Private Methods
        private IEnumerable<KeyValuePair<string, object>> GetObjectPropertyMappingForParse(object obj)
        {
            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            Type type = obj.GetType();
            foreach (PropertyInfo pi in type.GetRuntimeProperties())
            {
                IEnumerable<Attribute> propertyAttributes = pi.GetCustomAttributes();
                if (propertyAttributes.Any(a => a.GetType() == typeof(ParseObjectMemberAttribute)))
                {
                    result.Add(new KeyValuePair<string, object>(pi.Name, pi.GetValue(obj)));
                }
            }

            return result;
        }
        #endregion Private Methods

        #region Public Methods
        /// <summary>
        /// Method that returns a ParseObject object from a generic T object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be changed.</typeparam>
        /// <param name="obj">The object to be changed</param>
        /// <returns>A new ParseObject object</returns>
        public ParseObject GetParseObject<T>(T obj)
             where T : class, new()
        {

            if (object.ReferenceEquals(obj, null))
            {
                throw new ArgumentNullException("obj");
            }

            ParseObject po = new ParseObject(obj.GetType().Name);

            var propertiesMappingList = this.GetObjectPropertyMappingForParse(obj);

            foreach (var map in propertiesMappingList)
            {
                po.Add(map.Key, map.Value);
            }
            return po;
        }


        /// <summary>
        /// Method that returns a T object from a ParseObject. 
        /// The default result is a new instance of the T class created with Activator.
        /// </summary>
        /// <typeparam name="T">The type of object that will be returned.</typeparam>
        /// <param name="o">The ParseObject object</param>
        /// <returns>T object</returns>
        public T GetObjectFromParse<T>(ParseObject po)
            where T : class, new()
        {
            if (object.ReferenceEquals(po, null))
            {
                throw new ArgumentNullException("po");
            }


            T result = Activator.CreateInstance<T>();
            Type type = typeof(T);

            IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties();
            foreach (var pi in properties)
            {
                Attribute customAttribute = pi.GetCustomAttributes()
                                              .FirstOrDefault(a => a.GetType() == typeof(ParseObjectMemberAttribute));

                ParseObjectMemberAttribute casted = customAttribute as ParseObjectMemberAttribute;
                if (casted == null)
                {
                    continue;
                }

                if (casted.IsIdentity)
                {
                    pi.SetValue(result, po.ObjectId);
                }
                else
                {
                    string key = string.IsNullOrEmpty(casted.Key) ? pi.Name : casted.Key;

                    if (!po.ContainsKey(key))
                    {
                        continue;
                    }
                    pi.SetValue(result, po[key]);
                }
            }

            return result;
        }
        #endregion Public Methods
    }
}
