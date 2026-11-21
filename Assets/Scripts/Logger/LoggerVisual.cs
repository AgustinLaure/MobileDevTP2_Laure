using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoggerVisual : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject savedLogsGameObject;
    [SerializeField] private GameObject thrownLogsGameObject;
    [SerializeField] private Button showButton;
    [SerializeField] private Button clearButton;
    private GameManager gameManager;
    private LoggerTest loggerTest;

    int currentLogsLoaded = 0;

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;
        gameManager = serviceLocator.GetService<GameManager>();
        loggerTest = serviceLocator.GetService<LoggerTest>();
    }

    private void Start()
    {
        showButton.onClick.AddListener(HandleShowLogsPress);
        clearButton.onClick.AddListener(HandleClearButtonPressed);

#if UNITY_ANDROID && !UNITY_EDITOR
        if (loggerTest == null || loggerTest.loggerManagerInstance == null)
        {
            return;
        }

        string[] logs = loggerTest.loggerManagerInstance.Call<string[]>("GetLogs");

        if (logs != null && logs.Length != currentLogsLoaded)
        {
            for (int i = currentLogsLoaded; i < logs.Length; i++)
            {
                HandleLogReceive(logs[i]);
            }

            currentLogsLoaded = logs.Length;
        }
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void Update()
    {
        if (loggerTest == null || loggerTest.loggerManagerInstance == null)
        {
            return;
        }

        string[] logs = loggerTest.loggerManagerInstance.Call<string[]>("GetLogs");

        if (logs.Length != currentLogsLoaded)
        {
            for (int i = currentLogsLoaded; i < logs.Length; i++)
            {
                HandleLogReceive(logs[i]);
            }

            currentLogsLoaded = logs.Length;
        }
    }
#endif
    private void HandleLogReceive(string msg)
    {
        GameObject newText;

        if (thrownLogsGameObject != null)
        {
            newText = Instantiate(textPrefab, thrownLogsGameObject.transform, false);

            if (newText.TryGetComponent<TMP_Text>(out TMP_Text text1))
            {
                text1.text = msg;
            }
        }

        newText = Instantiate(textPrefab, savedLogsGameObject.transform, false);

        if (newText.TryGetComponent<TMP_Text>(out TMP_Text text2))
        {
            text2.text = msg;
        }

        Canvas.ForceUpdateCanvases();

        if (thrownLogsGameObject != null)
        {
            if (thrownLogsGameObject.TryGetComponent<RectTransform>(out RectTransform rectThrown))
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectThrown);
            }
        }

        if (savedLogsGameObject.TryGetComponent<RectTransform>(out RectTransform rectSaved))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectSaved);
        }

        gameManager.AddGameLog(newText);
    }

    private void HandleClearButtonPressed()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        loggerTest?.loggerManagerInstance?.Call("DeleteLogs");
#endif

        gameManager.DeleteGameLogs();
        currentLogsLoaded = 0;

        Canvas.ForceUpdateCanvases();

        if (savedLogsGameObject.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    private void HandleShowLogsPress()
    {
        Debug.Log("Log: " + loggerTest.GetLogCount);
        loggerTest.AddLogCount();
    }
    private void OnDestroy()
    {
        showButton.onClick.RemoveListener(HandleShowLogsPress);
        clearButton.onClick.RemoveListener(HandleClearButtonPressed);
    }
}