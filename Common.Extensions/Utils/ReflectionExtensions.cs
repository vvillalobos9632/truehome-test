using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Extensions.Utils
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetConcreteTypes(this Assembly assembly)
        {
            return assembly.GetLoadableTypes().Where(t => !(t.IsGenericType | t.IsGenericTypeDefinition));
        }

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentException("Assembly cannot be null");

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetAllInterfaces(this Type type)
        {
            return type.Assembly.GetConcreteTypes().Where(t => t.IsInterface);
        }

        public static IEnumerable<Type> GetAllImplementations(this Type type)
        {
            return type.Assembly.GetConcreteTypes(); ;
        }

        public static PropertyInfo[] GetPropertiesObject<T>()
        {
            Type objType = typeof(T);
            return objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
