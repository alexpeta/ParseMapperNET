using System;

namespace ParseMapperNET.Core
{
    /// <summary>
    /// The attribute used to decorate object properties that will be mapped against
    /// ParseObject objects
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParseObjectMemberAttribute : Attribute
    {
        public string Key { get; private set; }
        public bool IsIdentity { get; private set; }

        public ParseObjectMemberAttribute()
            : this(false, null)
        {
        }
        public ParseObjectMemberAttribute(bool isIdentity = false, string key = null)
        {
            Key = key;
            IsIdentity = isIdentity;
        }
    }
}
