namespace FazAppFramework.Development
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        ToDo,
        FrameworkInfo,
        FrameworkErrorInfo
    }

    public static class MainLogger
    {
        public static void Log(this object sender, string message, LogLevel logLevel)
        {
            string name = sender.GetType().Name;
            Log(name, message, logLevel);
        }
        
        public static void Log(string name, string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Debug(name, message);
                    break;
                case LogLevel.Info:
                    Info(name, message);
                    break;
                case LogLevel.Warning:
                    Warning(name, message);
                    break;
                case LogLevel.Error:
                    Error(name, message);
                    break;
                case LogLevel.ToDo:
                    ToDo(name, message);
                    break;
                case LogLevel.FrameworkInfo:
                    FrameworkInfo(name, message);
                    break;
                case LogLevel.FrameworkErrorInfo:
                    FrameworkErrorInfo(name, message);
                    break;
            }
        }

        private static void Debug(string senderName, string message)
        {
            UnityEngine.Debug.LogFormat("<color=black>{0} </color>: {1}", senderName, message);
        }

        private static void Info(string senderName, string message)
        {
            UnityEngine.Debug.LogFormat("<color=green>{0} </color>: {1}", senderName, message);
        }

        private static void Warning(string senderName, string message)
        {
            UnityEngine.Debug.LogWarningFormat("<color=yellow>{0} </color>: {1}", senderName, message);
        }

        private static void Error(string senderName, string message)
        {
            UnityEngine.Debug.LogErrorFormat("<color=red>{0} </color>: {1}", senderName, message);
        }
        
        private static void ToDo(string senderName, string message)
        {
            UnityEngine.Debug.LogFormat("<color=cyan>{0} TODO: </color> {1}", senderName, message);
        }

        private static void FrameworkInfo(string senderName, string message)
        {
            UnityEngine.Debug.LogFormat("<color=purple>FAZAPP FRAMEWORK - {0}</color>: {1}", senderName, message);
        }

        private static void FrameworkErrorInfo(string senderName, string message)
        {
            UnityEngine.Debug.LogFormat("<color=red>FAZAPP FRAMEWORK - {0}</color>: {1}", senderName, message);
        }

        private static void EditorInfo(string senderName, string message)
        {
            UnityEngine.Debug.LogFormat("<color=pink>{0} </color>: {1}", senderName, message);
        }
    }
}
