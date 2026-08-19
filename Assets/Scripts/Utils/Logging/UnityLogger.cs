using UnityEngine;

public class UnityLogger : ILogger
{
    public void LogError(string message)
    {
        LogService.LogErrorWithDetails(message);
    }

    public void LogInfo(string message)
    {
        LogService.LogWithDetails(message);
    }

    public void LogWarning(string message)
    {
        LogService.LogWarningWithDetails(message);
    }
}
