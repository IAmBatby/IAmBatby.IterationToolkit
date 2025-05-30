using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public enum TextStyle { Default, Bold, Italic }
    [System.Serializable]
    public class Logger
    {
        public string LogName { get; private set; }
        private List<string> activeLogLines = new List<string>();
        private List<string> activeRawLogLines = new List<string>();
        private int maxLogLines;

        public ExtendedEvent onLogAdded = new ExtendedEvent();

        public Logger(string newLogName, int newMaxLogLines)
        {
            LogName = newLogName;
            maxLogLines = newMaxLogLines;
        }

        public void Clear()
        {
            activeLogLines.Clear();
            activeRawLogLines.Clear();
        }

        public List<string> GetLogLines(bool getRaw = false, int maxLines = 0)
        {
            List<string> logLines;
            if (getRaw == false)
                logLines = new List<string>(activeLogLines);
            else
                logLines = new List<string>(activeRawLogLines);

            if (maxLines > 0 && logLines.Count > maxLines)
                logLines.RemoveRange(maxLines, logLines.Count - maxLines);

            return (logLines);
        }

        public void LogInfo(string messageName, string messageInfo)
        {
            LogInfo(messageName, messageInfo, Color.white);
        }

        public void LogInfo(string messageName, string messageInfo, Color infoColor)
        {
            string decoratedMessage = string.Empty;
            decoratedMessage += /*"<size=80%>" +*/  GetDecoratedIntro() + messageName.ToBold() + " ";
            if (!string.IsNullOrEmpty(messageInfo))
                decoratedMessage += /*"<size=70%>" +*/ messageInfo.ToItalic().Colorize(infoColor);

            AddLog(decoratedMessage, messageName + " " + messageInfo);
        }

        public void LogInfo(string message, float fontScale = 1.0f, TextStyle textStyle = TextStyle.Default)
        {
            LogInfo(message, Color.white, fontScale, textStyle);
        }

        public void LogInfo(string message, Color color, float fontScale = 1.0f, TextStyle textStyle = TextStyle.Default)
        {

            //string decoratedMessage = "<size=" + fontScale * 100 + "%>";
            string decoratedMessage = string.Empty;
            message = GetDecoratedIntro() + message;
            if (textStyle == TextStyle.Default)
                decoratedMessage += message.Colorize(color);
            else if (textStyle == TextStyle.Bold)
                decoratedMessage += message.ToBold().Colorize(color);
            else if (textStyle == TextStyle.Italic)
                decoratedMessage += message.ToItalic().Colorize(color);

            AddLog(decoratedMessage, message);
        }

        public void LogInfo(string message)
        {
            AddLog((GetDecoratedIntro() + message).Colorize(Color.white), message);
        }

        private string GetDecoratedIntro()
        {
            if (Time.time < 10f)
                return ("[0" + Time.time.ToString("F2") + "] ");
            else
                return ("[" + Time.time.ToString("F2") + "] ");
        }

        private void AddLog(string decoratedMessage, string rawMessage)
        {
            activeLogLines.Add(decoratedMessage);
            activeRawLogLines.Add(rawMessage);

            /*
            if (activeLogLines.Count == maxLogLines)
                activeLogLines.RemoveAt(0);
            if (activeRawLogLines.Count == maxLogLines)
                activeRawLogLines.RemoveAt(0);*/

            onLogAdded.Invoke();
        }

        public string GetLog()
        {
            string returnString = /*"<size=110%>" +*/ LogName.ToBold() + "\n" + "\n";

            foreach (string logLine in activeLogLines)
                returnString += logLine + "\n";

            return (returnString);
        }
    }
}
