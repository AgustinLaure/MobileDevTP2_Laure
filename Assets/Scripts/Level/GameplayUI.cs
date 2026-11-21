using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    private GameManager gameManager;
    private EventBus eventBus;

    [SerializeField] private CanvasGroup resultScreenCanvasGroup;
    [SerializeField] private Button resultMainMenuButton;
    [SerializeField] private Button resultRetryButton;
    [SerializeField] private Button resultContinueButton;

    [SerializeField] private CanvasGroup pauseScreenCanvasGroup;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button pauseMainMenuButton;
    [SerializeField] private Button pauseRetryButton;
    [SerializeField] private Button pauseResumeButton;

    [SerializeField] private CanvasGroup continueButtonCG;
    [SerializeField] private CanvasGroup pauseButtonCG;

    [SerializeField] private float disabledContinueAlpha;

    private void Awake()
    {
        resultMainMenuButton.onClick.AddListener(HandleResultMainMenu);
        resultRetryButton.onClick.AddListener(HandleResultRetry);
        resultContinueButton.onClick.AddListener(HandleResultContinue);

        pauseButton.onClick.AddListener(HandlePause);
        pauseMainMenuButton.onClick.AddListener(HandlePauseMainMenu);
        pauseRetryButton.onClick.AddListener(HandlePauseRetry);
        pauseResumeButton.onClick.AddListener(HandlePauseResume);

        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();
        eventBus = serviceLocator.GetService<EventBus>();

        eventBus.Subscribe<OnGameEnded>((Action<OnGameEnded>)HandleGameWin);
    }

    private void HandleGameWin(OnGameEnded data)
    {
        UiUtils.SetCanvasActive(resultScreenCanvasGroup, true);

        if (gameManager.IsCurrentLastLevel())
        {
            UiUtils.SetCanvasActive(continueButtonCG, false);
            continueButtonCG.alpha = disabledContinueAlpha;
        }

        gameManager.SetCurrentLevelWon();
        gameManager.SetPause(true);
    }

    private void HandleResultMainMenu()
    {
        gameManager.LoadScene(gameManager.GetMainMenuSceneName);
    }

    private void HandleResultRetry()
    {
        gameManager.SetPause(false);
        eventBus.Raise<OnRetry>();
        UiUtils.SetCanvasActive(resultScreenCanvasGroup, false);
    }

    private void HandleResultContinue()
    {
        gameManager.LoadLevel(gameManager.GetCurrentLevelIndex + 1);
    }

    private void HandlePauseMainMenu()
    {
        gameManager.SetPause(false);
        gameManager.LoadScene(gameManager.GetMainMenuSceneName);
    }

    private void HandlePause()
    {
        gameManager.SetPause(true);
        UiUtils.SetCanvasActive(pauseButtonCG, false);
        UiUtils.SetCanvasActive(pauseScreenCanvasGroup, true);
    }

    private void HandlePauseRetry()
    {
        gameManager.SetPause(false);
        eventBus.Raise<OnRetry>();
        UiUtils.SetCanvasActive(pauseScreenCanvasGroup, false);
    }

    private void HandlePauseResume()
    {
        gameManager.SetPause(false);
        UiUtils.SetCanvasActive(pauseButtonCG, true);
        UiUtils.SetCanvasActive(pauseScreenCanvasGroup, false);
    }

    private void OnDestroy()
    {
        resultMainMenuButton.onClick.RemoveListener(HandleResultMainMenu);
        resultRetryButton.onClick.RemoveListener(HandleResultRetry);
        resultContinueButton.onClick.RemoveListener(HandleResultContinue);

        pauseButton.onClick.RemoveListener(HandlePause);
        pauseMainMenuButton.onClick.RemoveListener(HandlePauseMainMenu);
        pauseRetryButton.onClick.RemoveListener(HandlePauseRetry);
        pauseResumeButton.onClick.RemoveListener(HandlePauseResume);

        eventBus.Unsubscribe<OnGameEnded>((Action<OnGameEnded>)HandleGameWin);
    }
}