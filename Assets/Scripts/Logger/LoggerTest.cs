using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;

public class LoggerTest : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public AndroidJavaObject loggerManagerInstance;
#endif
    private readonly ConcurrentQueue<string> pendingLogs = new();

    private int logCount = 0;

    public int GetLogCount { get { return logCount; } }

    private void Start()
    {
        Application.logMessageReceived += HandleLogReceive;

#if UNITY_ANDROID && !UNITY_EDITOR
        using var loggerClass = new AndroidJavaClass("com.example.proj_logger.LoggerManager");
        loggerManagerInstance = loggerClass.CallStatic<AndroidJavaObject>("GetInstance");
#endif
    }

    private void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        while (pendingLogs.TryDequeue(out var msg))
        {
            loggerManagerInstance?.Call("SendLog", msg);
        }
#endif
    }

    public void AddLogCount()
    {
        logCount++;
    }

    private void HandleLogReceive(string logString, string stackTrace, LogType type)
    {
        pendingLogs.Enqueue(logString);
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLogReceive;
    }
}