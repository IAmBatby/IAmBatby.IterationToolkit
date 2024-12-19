using Codice.LogWrapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public static class DynamicConsole
    {
        private static Dictionary<string, Logger> loggerDict = new Dictionary<string, Logger>();
        private static List<Logger> loggerList = new List<Logger>();

        public static SelectableCollection<Logger> ActiveLogs { get; private set; }
        public static int MaxLines { get; private set; } = 12;
        public static ExtendedEvent OnLoggerModified { get; private set; } = new ExtendedEvent();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            OnLoggerModified = new ExtendedEvent();
            loggerDict.Clear();
            loggerList.Clear();
            ActiveLogs = null;
        }

        public static bool TryAddLogger(string loggerName, out Logger newLogger)
        {
            newLogger = AddLogger(loggerName);
            return (newLogger != null);
        }

        public static Logger AddLogger(string loggerName)
        {
            Logger newLogger = null;
            if (loggerDict.ContainsKey(loggerName))
            {
                Debug.LogWarning("Can't initialize logger as it's logname already initialized!");
                return (newLogger);
            }
            newLogger = new Logger(loggerName, MaxLines);
            loggerDict.Add(loggerName, newLogger);
            loggerList.Add(newLogger);
            if (ActiveLogs == null)
                ActiveLogs = new SelectableCollection<Logger>(loggerList);
            else
                ActiveLogs.AddObject(newLogger);
            return (newLogger);
        }

        public static List<Logger> GetLoggers() => new List<Logger>(loggerList);

        public static bool TryGetLogger(string loggerName, out Logger logger)
        {
            return (loggerDict.TryGetValue(loggerName, out logger));
        }

        public static void ClearLog(Logger logger)
        {
            logger.activeLogLines.Clear();
        }

        public static void AddLog(Logger logger, string message)
        {
            logger.LogInfo(message);
        }

        public static string GetActiveLog()
        {
            return (ActiveLogs.ActiveSelection.GetLog());
        }

        public static void ToggleForward()
        {
            ActiveLogs.SelectForward();
        }

        public static void ToggleBackward()
        {
            ActiveLogs.SelectBackward();
        }

        public static void InvokeLogModified()
        {
            OnLoggerModified.Invoke();
        }

        public static Logger Register(string logName, ref List<Logger> loggers)
        {
            Logger newLogger = new Logger(logName, MaxLines);
            loggers.Add(newLogger);
            return (newLogger);
        }
    }
}
