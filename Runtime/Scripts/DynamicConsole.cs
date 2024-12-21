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

        private static Vector2 consoleSizeScale = new Vector2(2f, 4.25f);

        private static Vector2 consoleScrollView;

        private static int previousCount;

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
            logger.Clear();
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

        public static void SetRenderedConsoleValues(float newWidthScale, float newHeightScale)
        {
            consoleSizeScale.y = newHeightScale;
            consoleSizeScale.x = newWidthScale;
        }

        public static void RenderConsole()
        {
            float smallest = Mathf.Min(Screen.width, Screen.height);
            float consoleWidth = smallest / consoleSizeScale.x;
            float consoleHeight = smallest / consoleSizeScale.y;

            Rect rect = new Rect(0 + (consoleWidth / 12.5f), Screen.height - (consoleHeight + (consoleHeight / 4.5f)), consoleWidth, consoleHeight);
            GUILayout.BeginArea(rect, GUIDefaults.UI_Background);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label(ActiveLogs.ActiveSelection.LogName.ToBold().Colorize(Color.white), GUIDefaults.UI_Header);
            GUILayout.Space(10);
            List<string> logLines = ActiveLogs.ActiveSelection.GetLogLines();
            if (logLines.Count > previousCount)
                consoleScrollView = new Vector2(0, 9999999999);
            consoleScrollView = GUILayout.BeginScrollView(consoleScrollView, false, alwaysShowVertical: true);
            for (int i = 0; i < logLines.Count; i++)
                    GUILayout.Label(logLines[i], GUIDefaults.UI_Label);

            previousCount = logLines.Count;
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUILayout.FlexibleSpace();
        }
    }
}
