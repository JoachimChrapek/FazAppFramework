using System;
using System.Linq;
using FazAppFramework.Attributes;
using FazAppFramework.Development;
using FazAppFramework.Reflection;
using UnityEngine;

namespace FazAppFramework
{
    public static class MainBehaviourExtensions
    {
        public static void CheckSerializedFields<T>(this T behaviour) where T : MainBehaviour
        {
            foreach (var field in behaviour.GetFieldsWithAttribute<T, SerializeField>().Where(f => f.GetValue(behaviour) == null))
            {
                behaviour.Log($"SerializedField {field.Name} not assigned", LogLevel.Error);
            }
        }

        public static void GetComponents<T>(this T behaviour) where T : MainBehaviour
        {
            foreach (var field in behaviour.GetFieldsWithAttribute<T, GetComponentAttribute>().Where(f => f.GetValue(behaviour) == null))
            {
                try
                {
                    field.SetValue(behaviour, behaviour.GetComponent(field.FieldType));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting field {field.FieldType}, {field.Name} in {typeof(T)}: {e}");
                }
            }

            foreach (var property in behaviour.GetPropertiesWithAttribute<T, GetComponentAttribute>().Where(p => p.GetValue(behaviour) == null))
            {
                try
                {
                    property.SetValue(behaviour, behaviour.GetComponent(property.PropertyType));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting property {property.PropertyType}, {property.Name} in {typeof(T)}: {e}");
                }
            }

            foreach (var field in behaviour.GetFieldsWithAttribute<T, GetComponentInChildrenAttribute>().Where(f => f.GetValue(behaviour) == null))
            {
                try
                {
                    field.SetValue(behaviour, behaviour.GetComponentInChildren(field.FieldType, true));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting field {field.FieldType}, {field.Name} in {typeof(T)}: {e}");
                }
            }

            foreach (var property in behaviour.GetPropertiesWithAttribute<T, GetComponentInChildrenAttribute>().Where(p => p.GetValue(behaviour) == null))
            {
                try
                {
                    property.SetValue(behaviour, behaviour.GetComponentInChildren(property.PropertyType, true));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting property {property.PropertyType}, {property.Name} in {typeof(T)}: {e}");
                }
            }

            foreach (var field in behaviour.GetFieldsWithAttribute<T, GetComponentInParentAttribute>().Where(f => f.GetValue(behaviour) == null))
            {
                try
                {
                    field.SetValue(behaviour, behaviour.GetComponentInParent(field.FieldType));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting field {field.FieldType}, {field.Name} in {typeof(T)}: {e}");
                }
            }

            foreach (var property in behaviour.GetPropertiesWithAttribute<T, GetComponentInParentAttribute>().Where(p => p.GetValue(behaviour) == null))
            {
                try
                {
                    property.SetValue(behaviour, behaviour.GetComponentInParent(property.PropertyType));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception occured during injecting property {property.PropertyType}, {property.Name} in {typeof(T)}: {e}");
                }
            }
        }
    }
}
