using System;
using System.Diagnostics;
using System.Reflection;

namespace MySystem.Base.Reflection
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Get property value from object
        /// </summary>
        /// <param name="instance">Object instance that hosts the property</param>
        /// <param name="path">Path to the property</param>
        /// <returns>Value of the property</returns>
        /// <remarks>Works for pblic and private properties on the path</remarks>
        /// <example>GetPropertyValue( obj, "Item.SubItem.Name" )</example>
        public static object GetPropertyValue(this object instance, string path)
        {
            if (string.IsNullOrEmpty(path)) return instance;

            // Walk all way down the path
            foreach (var element in path.Split('.'))
            {
                var type = instance.GetType();
                var property = type.GetProperty(element, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                Debug.Assert(
                    property != null,
                    string.Format("Can't find property {0} in object of type {1}.",
                        element,
                        type.FullName));

                instance = property.GetValue(instance, null);
            }

            return instance;
        }
        public static object GetPropertyValue(this object instance, string path, out PropertyInfo property)
        {
            property = null;
            if (string.IsNullOrEmpty(path)) return instance;

            // Walk all way down the path
            foreach (var element in path.Split('.'))
            {
                var type = instance.GetType();
                property = type.GetProperty(element, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                Debug.Assert(
                    property != null,
                    string.Format("Can't find property {0} in object of type {1}.",
                        element,
                        type.FullName));

                instance = property.GetValue(instance, null);
            }

            return instance;
        }

        public static void SetPropertyValue(this object obj, string propName, object value)
        {
            obj.GetType().GetProperty(propName)?.SetValue(obj, value, null);
        }
    }
}