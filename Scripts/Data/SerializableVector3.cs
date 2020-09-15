using System;
using UnityEngine;

namespace FazAppFramework.Data
{
    [Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float x, float y, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SerializableVector3(Vector3 vector3)
        {
            this.x = vector3.x;
            this.y = vector3.y;
            this.z = vector3.z;
        }

        public Vector3 ToUnityVector()
        {
            return new Vector3(x, y, z);
        }
    }
}

