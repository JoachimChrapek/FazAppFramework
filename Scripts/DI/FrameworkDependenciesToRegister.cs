using FazAppFramework.Managers;

namespace FazAppFramework.DI
{
    internal static class FrameworkDependenciesToRegister
    {
        public static void GetTypes(DependencyRegisterModule registerModule)
        {
            registerModule.AddTypeToRegister(typeof(FirebaseManager), true, null, true);
            registerModule.AddTypeToRegister(typeof(AdManager), true, null, true);
            registerModule.AddTypeToRegister(typeof(FirebaseOwnInterstitial), true, null, true);
            registerModule.AddTypeToRegister(typeof(LocalNotificationManager), true, null, true);

        }

        public static void GetObjects(DependencyRegisterModule registerModule)
        {
            var frameworkValues = FrameworkValuesHelper.GetFrameworkValues();
            registerModule.AddObjectToRegister(frameworkValues, null, true);


        }
    }
}
