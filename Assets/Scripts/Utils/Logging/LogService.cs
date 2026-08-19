using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class LogService
{
    public static void Log(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Debug.Log($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber} - {memberName}] {message}");
    }

    public static void LogWarning(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Debug.LogWarning($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber} - {memberName}] {message}");
    }

    public static void LogError(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Debug.LogError($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber} - {memberName}] {message}");
    }
}
