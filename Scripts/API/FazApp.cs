using System;
using FazAppFramework.Managers;
using UnityEngine;

namespace FazAppFramework
{
    public static class FazApp
    {
        private static readonly InitializationModule initializationModule = new InitializationModule();

        /// <summary>
        /// Initializes FazApp Framework and all services in it
        /// </summary>
        /// <param name="onInitializationComplete">Action to invoke after completed initialization</param>
        public static async void Activate(Action onInitializationComplete)
        {
            await initializationModule.InitializeFramework(onInitializationComplete);
        }

        /// <summary>
        /// Adds a Type to register in dependency registration.
        /// </summary>
        /// <param name="type">Type to register</param>
        /// <param name="isSingletone">Defines if object should be treated as singletone (single instance with every resolve)</param>
        /// <param name="additionalTypes">Additional types to register object as</param>
        public static void AddTypeToDependencyRegister(Type type, bool isSingletone, params Type[] additionalTypes)
        {
            initializationModule.DependencyRegisterModule.AddTypeToRegister(type, isSingletone, additionalTypes);
        }

        /// <summary>
        /// Adds an object instance that is created before dependency registration. Use it for ScriptableObjects and GameObjects
        /// </summary>
        /// <param name="obj">Object to register</param>
        /// <param name="additionalTypes">Additional types to register object as</param>
        public static void AddObjectToDependencyRegister(object obj, params Type[] additionalTypes)
        {
            initializationModule.DependencyRegisterModule.AddObjectToRegister(obj, additionalTypes);
        }
        
        /// <summary>
        /// Adds component to GlobalManager which is game object that don't destroys on scenes load.
        /// </summary>
        public static void AddComponentToGlobalManager<T>() where T : Component
        {
            initializationModule.TypesToAddToGlobalManager.Add(typeof(T));
        }

        /// <summary>
        /// Adds an instantiated game object as child of GlobalManager which is game object that don't destroys on scenes load.
        /// </summary>
        /// <param name="gameObject">Object on scene to add</param>
        public static void AddGameObjectToGlobalManager(GameObject gameObject)
        {
            initializationModule.GameObjectsToAddToGlobalManager.Add(gameObject);
        }
    }
}
