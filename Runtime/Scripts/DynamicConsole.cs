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
        /*
        public static void SetLoggers(List<Logger> loggers, int newMaxLines)
        {
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
        }*/

        public static bool TryAddLogger(string loggerName, out Logger newLogger)
        {
            newLogger = null;
            if (loggerDict.ContainsKey(loggerName))
            {
                Debug.LogWarning("Can't initialize logger as it's logname already initialized!");
                return (false);
            }
            newLogger = new Logger(loggerName, MaxLines);
            loggerDict.Add(loggerName, newLogger);
            loggerList.Add(newLogger);
            if (ActiveLogs == null)
                ActiveLogs = new SelectableCollection<Logger>(loggerList);
            ActiveLogs.AddObject(newLogger);

            return (true);
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

        public static Logger Register(string logName, ref List<Logger> loggers)
        {
            Logger newLogger = new Logger(logName, MaxLines);
            loggers.Add(newLogger);
            return (newLogger);
        }
    }
}
