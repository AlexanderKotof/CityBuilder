using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.example
{
    public static class Logger
    {
        private const string _logFormat = "[{0} : {1}] ({2}) {3}";
		
        public static void Log(string message, Object context = null,
            [CallerFilePath] string filePath = null,
            [CallerMemberName] string methodName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            Debug.Log(Format(message, filePath, methodName, lineNumber), context);
        }
		
        public static void LogWarning(string message, Object context = null,
            [CallerFilePath] string filePath = null,
            [CallerMemberName] string methodName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            Debug.LogWarning(Format(message, filePath, methodName, lineNumber), context);
        }
		
        public static void LogError(string message, Object context = null,
            [CallerFilePath] string filePath = null,
            [CallerMemberName] string methodName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            Debug.LogError(Format(message, filePath, methodName, lineNumber), context);
        }

        private static string Format(string message, string filePath, string methodName, int lineNumber)
        {
            return string.Format(_logFormat, Path.GetFileName(filePath), lineNumber, methodName, message);
        }

        public static void LogException(Exception exception, Object context = null,
            [CallerFilePath] string filePath = null,
            [CallerMemberName] string methodName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            Debug.LogError(Format(exception.Message, filePath, methodName, lineNumber), context);
            Debug.LogException(exception, context);
        }
    }
}