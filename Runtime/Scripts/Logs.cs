using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = IterationToolkit.Logger;

[System.Serializable]
public class Logs
{
    private List<Logger> loggers = new List<Logger>();
    private int defaultMaxLines;

    public Logs(int maxLines)
    {
        defaultMaxLines = maxLines;
    }

    public Logger AddLogger(string loggerName)
    {
        Logger logger = new Logger(loggerName, defaultMaxLines);
        loggers.Add(logger);
        return (logger);
    }
}
