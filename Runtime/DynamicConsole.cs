using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DynamicConsole
{
    public ExtendedEvent onLoggerModified = new ExtendedEvent();

    public SelectableCollection<Logger> selectableLogger;

    public DynamicConsole(int maxLines)
    {
        InitializeLoggers(GetLoggers(maxLines));
    }

    public virtual List<Logger> GetLoggers(int maxLines)
    {
        return (new List<Logger>());
    }

    public void InitializeLoggers(List<Logger> loggers)
    {
        selectableLogger = new SelectableCollection<Logger>(loggers);
        selectableLogger.AssignOnSelected(InvokeLogModified);
        foreach (Logger logger in loggers)
            logger.onLogAdded.AddListener(InvokeLogModified);
    }

    public void ClearLog(Logger logger)
    {
        logger.activeLogLines.Clear();
    }

    public void AddLog(Logger logger, string message)
    {
        logger.LogInfo(message);
    }

    public string GetActiveLog()
    {
        return (selectableLogger.ActiveSelection.GetLog());
    }

    public void ToggleForward()
    {
        selectableLogger.SelectForward();
    }

    public void ToggleBackward()
    {
        selectableLogger.SelectBackward();
    }

    public void InvokeLogModified()
    {
        onLoggerModified.Invoke();
    }
}
