using System;
using System.Linq;
using System.Reflection;

namespace FazAppFramework.Reflection
{
    public static class ReflectionExtensions
    {
        private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static FieldInfo[] GetFieldsWithAttribute<T, TA>(this T obj) where TA : Attribute
        {
            return obj.GetType().GetFields(DefaultFlags).Where(f => f.IsDefined(typeof(TA), false)).ToArray();
        }

        public static PropertyInfo[] GetPropertiesWithAttribute<T, TA>(this T obj) where TA : Attribute
        {
            return obj.GetType().GetProperties(DefaultFlags).Where(p => p.IsDefined(typeof(TA), false)).ToArray();
        }

        public static MethodInfo[] GetMethodsWithAttribute<T, TA>(this T obj) where TA : Attribute
        {
            return obj.GetType().GetMethods(DefaultFlags).Where(m => m.IsDefined(typeof(TA), false)).ToArray();
        }

        public static FieldInfo[] GetAssignableFields<T, TF>(this T obj)
        {
            return obj.GetType().GetFields(DefaultFlags).Where(f => typeof(TF).IsAssignableFrom(f.FieldType)).ToArray();
        }

        public static PropertyInfo[] GetAssignableProperties<T, TP>(this T obj)
        {
            return obj.GetType().GetProperties(DefaultFlags).Where(f => typeof(TP).IsAssignableFrom(f.PropertyType)).ToArray();
        }
    }
}
