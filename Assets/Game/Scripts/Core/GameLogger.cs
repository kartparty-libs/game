using Framework;

public class GameLogger
{
    private DebugLogger logger = new DebugLogger();
    [System.Diagnostics.Conditional("LogError")]
    public void LogErrorFormat(string format, params object[] args)
    {
        logger?.LogErrorFormat(format, args);
    }
    [System.Diagnostics.Conditional("LogError")]
    public void LogError(object message)
    {
        logger?.LogError(message);
    }
    [System.Diagnostics.Conditional("Log")]
    public void LogFormat(string format, params object[] args)
    {
        logger?.LogFormat(format, args);
    }
    [System.Diagnostics.Conditional("Log")]
    public void Log(object message)
    {
        logger?.Log(message);
    }
    [System.Diagnostics.Conditional("LogWarning")]
    public void LogWarning(object message)
    {
        logger?.LogWarning(message);
    }
    [System.Diagnostics.Conditional("LogWarning")]
    public void LogWarningFormat(string format, params object[] args)
    {
        logger?.LogWarningFormat(format, args);
    }
    public void Update()
    {
        logger?.Update();
    }
}
