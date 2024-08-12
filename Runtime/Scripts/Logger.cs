using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace IterationToolkit
{
    public enum TextStyle { Default, Bold, Italic }
    [System.Serializable]
    public class Logger
    {
        public string logName;
        public List<string> activeLogLines = new List<string>();
        public List<string> activeRawLogLines = new List<string>();
        public int maxLogLines;

        public ExtendedEvent onLogAdded = new ExtendedEvent();

        public Logger(string newLogName, int newMaxLogLines)
        {
            logName = newLogName;
            maxLogLines = newMaxLogLines;
        }

        public void Clear()
        {
            activeLogLines.Clear();
            activeRawLogLines.Clear();
        }

        public void LogInfo(string messageName, string messageInfo)
        {
            LogInfo(messageName, messageInfo, Color.white);
        }

        public void LogInfo(string messageName, string messageInfo, Color infoColor)
        {
            string decoratedMessage = string.Empty;
            decoratedMessage += "<size=80%>" + messageName.ToBold() + " ";
            if (!string.IsNullOrEmpty(messageInfo))
                decoratedMessage += "<size=70%>" + messageInfo.ToItalic().Colorize(infoColor);

            AddLog(decoratedMessage, messageName + " " + messageInfo);
        }

        public void LogInfo(string message, float fontScale = 1.0f, TextStyle textStyle = TextStyle.Default)
        {
            LogInfo(message, Color.white, fontScale, textStyle);
        }

        public void LogInfo(string message, Color color, float fontScale = 1.0f, TextStyle textStyle = TextStyle.Default)
        {

            string decoratedMessage = "<size=" + fontScale * 100 + "%>";
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
            AddLog(message, message);
        }

        private void AddLog(string decoratedMessage, string rawMessage)
        {
            activeLogLines.Add(decoratedMessage);
            activeRawLogLines.Add(rawMessage);

            if (activeLogLines.Count == maxLogLines)
                activeLogLines.RemoveAt(0);
            if (activeRawLogLines.Count == maxLogLines)
                activeRawLogLines.RemoveAt(0);

            onLogAdded.Invoke();
        }

        public string GetLog()
        {
            string returnString = "<size=110%>" + logName.ToBold() + "\n" + "\n";

            foreach (string logLine in activeLogLines)
                returnString += logLine + "\n";

            return (returnString);
        }
    }
}
