using System;
using System.Linq;
using FazAppFramework.Attributes;
using FazAppFramework.Reflection;

namespace FazAppFramework.DI
{
    public static class DependencyInjectionExtensions
    {
        public static void ResolveDependencies<T>(this T obj)
        {
            foreach (var field in obj.GetFieldsWithAttribute<T, DependencyInjectAttribute>().Where(f => f.GetValue(obj) == null))
            {
                try
                {
                    field.SetValue(obj, Master.Resolve(field.FieldType));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting field {field.FieldType} {field.Name} in {typeof(T)}: {e}");
                }
            }

            foreach (var property in obj.GetPropertiesWithAttribute<T, DependencyInjectAttribute>().Where(p => p.GetValue(obj) == null))
            {
                try
                {
                    property.SetValue(obj, Master.Resolve(property.PropertyType));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting property {property.PropertyType} {property.Name} in {typeof(T)}: {e}");
                }
            }
        }
    }
}
