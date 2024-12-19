using Codice.LogWrapper;
using System.Buffers;
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

        private static float consoleWidthScale = 2f;
        private static float consoleHeightScale = 4.25f;

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
            consoleHeightScale = newHeightScale;
            consoleWidthScale = newWidthScale;
        }

        public static void RenderConsole()
        {
            Color backgroundColor = Color.black.SetAlpha(0.45f);
            GUIStyle backgroundStyle = LabelUtilities.CreateStyle(enableRichText: true, backgroundColor);
            //GUIStyle logStyle = LabelUtilities.CreateStyle(enableRichText: true, new Color(0,0,0,0));
            GUIStyle labelStyle = LabelUtilities.CreateStyle(true);
            GUIStyle headerStyle = LabelUtilities.CreateStyle(true);
            labelStyle.richText = true;
            headerStyle.fontSize = 18;
            //labelStyle.normal.textColor = Color.white;
            float smallest = Mathf.Min(Screen.width, Screen.height);

            float consoleWidth = smallest / consoleWidthScale;
            float consoleHeight = smallest / consoleHeightScale;

            Rect rect = new Rect(0 + (consoleWidth / 12.5f), Screen.height - (consoleHeight + (consoleHeight / 4.5f)), consoleWidth, consoleHeight);
            GUILayout.BeginArea(rect, backgroundStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label(ActiveLogs.ActiveSelection.LogName.ToBold().Colorize(Color.white), headerStyle);
            GUILayout.Space(10);
            List<string> logLines = ActiveLogs.ActiveSelection.GetLogLines();
            for (int i = 0; i < MaxLines; i++)
                if (i < logLines.Count)
                    GUILayout.Label(GetMessageStart(i) + logLines[i], labelStyle);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private static string GetMessageStart(int index) => (("[" + Time.time.ToString("F2") + "] ".ToBold()).Colorize(Color.white));
    }
}
