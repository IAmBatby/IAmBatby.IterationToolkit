using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public static class DynamicConsole
    {
        public static ExtendedEvent OnLoggerModified { get; private set; } = new ExtendedEvent();

        public static SelectableCollection<Logger> ActiveLogs { get; private set; }
        private static Dictionary<string, Logger> loggerDict = new Dictionary<string, Logger>();
        private static List<Logger> loggerList = new List<Logger>();
        private static int maxLines = 12;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            loggerDict.Clear();
            loggerList.Clear();
            ActiveLogs = null;
        }

        public static void SetLoggers(List<Logger> loggers, int newMaxLines)
        {
            OnLoggerModified = new ExtendedEvent();
            loggerList.Clear();
            loggerDict.Clear();
            maxLines = newMaxLines;
            foreach (Logger logger in loggers)
            {
                if (loggerDict.ContainsKey(logger.logName))
                    Debug.LogWarning("Can't initialize logger as it's logname already initialized!");
                else
                    loggerDict.Add(logger.logName, logger);
            }

            foreach (KeyValuePair<string, Logger> kvp in loggerDict)
            {
                loggerList.Add(kvp.Value);
                kvp.Value.onLogAdded.AddListener(InvokeLogModified);
            }
            ActiveLogs = new SelectableCollection<Logger>(loggers);
            ActiveLogs.AssignOnSelected(InvokeLogModified);
        }

        public static List<Logger> GetLoggers() => new List<Logger>(loggerList);

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
    }
}
