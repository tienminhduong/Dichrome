using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class LogService
{
    private static Dictionary<Type, ILogger> loggers;
    public static ILogger GetLogger<T>() where T : ILogger, new()
    {
        loggers ??= new Dictionary<Type, ILogger>
            {
                { typeof(T), new T() }
            };

        if (!loggers.ContainsKey(typeof(T)))
        {
            loggers.Add(typeof(T), new T());
        }

        return loggers[typeof(T)];

    }

    public static ILogger Default => GetLogger<UnityLogger>();

    public static void Log(string message) => Default.LogInfo(message);
    public static void LogWarning(string message) => Default.LogWarning(message);
    public static void LogError(string message) => Default.LogError(message);

    public static void LogWithDetails(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Debug.Log($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber} - {memberName}] {message}");
    }

    public static void LogWarningWithDetails(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Debug.LogWarning($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber} - {memberName}] {message}");
    }

    public static void LogErrorWithDetails(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Debug.LogError($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber} - {memberName}] {message}");
    }
}
