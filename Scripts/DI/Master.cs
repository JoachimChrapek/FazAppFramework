using System;
using System.Linq;
using System.Reflection;
using Autofac;

namespace FazAppFramework.DI
{
    public static class Master
    {
        private static IContainer _container;
        
        internal static void Initialize(DependencyRegisterModule registerModule)
        {
            if(_container != null)
                return;

            CreateContainer(registerModule);
        }

        private static void CreateContainer(DependencyRegisterModule registerModule)
        {
            try
            {
                var builder = new ContainerBuilder();

                registerModule.Register(builder);

                _container = builder.Build();
            }
            catch (Exception e)
            {
                throw new Exception("Exception occured during container creation. \n" +
                                    $"{e.Message} \n" +
                                    "For more information - catch stack.");
            }
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return _container.ResolveNamed<object>(type.FullName);
        }
    }
}
