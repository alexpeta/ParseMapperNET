using Parse;

namespace ParseMapperNET.Portable
{
    public interface IParseMapper
    {
        /// <summary>
        /// Method that returns a ParseObject object from a generic T object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be changed.</typeparam>
        /// <param name="obj">The object to be changed</param>
        /// <returns>A new ParseObject object</returns>
        ParseObject GetParseObject<T>(T o) where T : class, new();

        /// <summary>
        /// Method that returns a T object from a ParseObject. 
        /// The default result is a new instance of the T class created with Activator.
        /// </summary>
        /// <typeparam name="T">The type of object that will be returned.</typeparam>
        /// <param name="o">The ParseObject object</param>
        /// <returns>T object</returns>
        T GetObjectFromParse<T>(ParseObject po) where T : class, new();
    }
}
