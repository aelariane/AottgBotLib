using System;

namespace AottgBotLib
{
    public static class Log
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        public static Action<string, LogLevel> LogMethod = null;

        public static void LogInfo(string message)
        {
            LogMethod?.Invoke(message, LogLevel.Info);
        }


        public static void LogWarning(string message)
        {
            LogMethod?.Invoke(message, LogLevel.Warning);
        }

        public static void LogError(string message)
        {
            LogMethod?.Invoke(message, LogLevel.Error);
        }
    }
}
