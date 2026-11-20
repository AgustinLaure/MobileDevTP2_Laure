using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    private GameManager gameManager;
    private EventBus eventBus;

    [SerializeField] private CanvasGroup resultScreenCanvasGroup;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(HandleMainMenu);
        retryButton.onClick.AddListener(HandleRetry);
        continueButton.onClick.AddListener(HandleContinue);

        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();
        eventBus = serviceLocator.GetService<EventBus>();

        eventBus.Subscribe<OnGameEnded>((Action<OnGameEnded>)HandleGameEnd);
    }

    private void HandleGameEnd(OnGameEnded data)
    {
        UiUtils.SetCanvasActive(resultScreenCanvasGroup, true);
    }

    private void HandleMainMenu()
    {
        gameManager.LoadScene(gameManager.GetMainMenuSceneName);
    }

    private void HandleRetry()
    {
        eventBus.Raise<OnRetry>();
        UiUtils.SetCanvasActive(resultScreenCanvasGroup, false);
    }

    private void HandleContinue()
    {

    }

    private void OnDestroy()
    {
        mainMenuButton.onClick.RemoveListener(HandleMainMenu);
        retryButton.onClick.RemoveListener(HandleRetry);
        continueButton.onClick.RemoveListener(HandleContinue);

        eventBus.Unsubscribe<OnGameEnded>((Action<OnGameEnded>)HandleGameEnd);
    }
}