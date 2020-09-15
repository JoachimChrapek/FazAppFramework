using System;
using System.Collections.Generic;
using Autofac;

namespace FazAppFramework.DI
{
    internal class DependencyRegisterModule
    {
        private class TypeToRegister
        {
            public Type type;
            public bool isSingletone;
            public Type[] additionalTypes;
        }

        private class ObjectToRegister
        {
            public object obj;
            public Type[] additionalTypes;
        }

        private readonly List<TypeToRegister> frameworkTypesToRegister = new List<TypeToRegister>();
        private readonly List<TypeToRegister> typesToRegister = new List<TypeToRegister>();
        private readonly List<ObjectToRegister> frameworkObjectsToRegister = new List<ObjectToRegister>();
        private readonly List<ObjectToRegister> objectsToRegister = new List<ObjectToRegister>();
        
        public void Register(ContainerBuilder containerBuilder)
        {
            FrameworkDependenciesToRegister.GetTypes(this);
            FrameworkDependenciesToRegister.GetObjects(this);

            RegisterTypes(containerBuilder);
            RegisterObjects(containerBuilder);
        }

        public void AddTypeToRegister(
            Type type, 
            bool isSingletone, 
            Type[] additionalTypes, 
            bool typeFromFramework = false)
        {
            var toRegister = new TypeToRegister
            {
                type = type,
                isSingletone = isSingletone,
                additionalTypes = additionalTypes
            };

            if (typeFromFramework)
            {
                frameworkTypesToRegister.Add(toRegister);
            }
            else
            {
                typesToRegister.Add(toRegister);
            }
        }

        public void AddObjectToRegister(
            object obj,
            Type[] additionalTypes,
            bool objectFromFramework = false)
        {
            var toRegister = new ObjectToRegister
            {
                obj = obj,
                additionalTypes = additionalTypes
            };

            if (objectFromFramework)
            {
                frameworkObjectsToRegister.Add(toRegister);
            }
            else
            {
                objectsToRegister.Add(toRegister);
            }
        }

        private void RegisterTypes(ContainerBuilder containerBuilder)
        {
            foreach (var typeToRegister in frameworkTypesToRegister)
            {
                RegisterType(typeToRegister, containerBuilder);
            }

            foreach (var typeToRegister in typesToRegister)
            {
                RegisterType(typeToRegister, containerBuilder);
            }
        }

        private void RegisterType(TypeToRegister typeToRegister, ContainerBuilder containerBuilder)
        {
            var registration = containerBuilder.RegisterType(typeToRegister.type).AsSelf();
            registration.As(typeToRegister.additionalTypes);

            if (typeToRegister.isSingletone)
            {
                registration.SingleInstance();
            }
            else
            {
                registration.InstancePerDependency();
            }
        }

        private void RegisterObjects(ContainerBuilder containerBuilder)
        {
            foreach (var objectToRegister in frameworkObjectsToRegister)
            {
                RegisterObject(objectToRegister, containerBuilder);
            }

            foreach (var objectToRegister in objectsToRegister)
            {
                RegisterObject(objectToRegister, containerBuilder);
            }
        }

        private void RegisterObject(ObjectToRegister objectToRegister, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(objectToRegister.obj)
                .As(objectToRegister.GetType())
                .SingleInstance()
                .As(objectToRegister.additionalTypes);
        }
    }
}
