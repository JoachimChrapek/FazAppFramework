using FazAppFramework.Data;
using UnityEngine;

namespace FazAppFramework.Extensions
{
    public static class DataExtensions
    {
        public static SerializableVector3 ToSerializableVector(this Vector3 v)
        {
            return new SerializableVector3(v);
        }
    }
}
