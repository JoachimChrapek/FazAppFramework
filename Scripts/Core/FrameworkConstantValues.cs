using UnityEngine;

namespace FazAppFramework
{
    public static class FrameworkConstantValues
    {
        public const string FrameworkValuesFileName = "FrameworkValues.json";
        public static readonly string FrameworkValuesFilePath = Application.dataPath + "/" + FrameworkValuesFileName;

        public const string SESSION_COUNTER_KEY = "SessionCounter";

        public const string RATE_US_DONE_KEY = "RateUsDone";
    }
}
