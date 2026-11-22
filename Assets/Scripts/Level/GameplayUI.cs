using System;
using TMPro;
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

    [SerializeField] private TMPro.TMP_Text gameplayCoinAmountText;
    [SerializeField] private TMPro.TMP_Text gameplayTimerText;

    [SerializeField] private TMPro.TMP_Text resultCoinAmountText;
    [SerializeField] private TMPro.TMP_Text resultTimerText;

    [SerializeField] private float disabledContinueAlpha;

    [SerializeField] private CanvasGroup[] heartImages;
    [SerializeField] private float lostHealthAlpha;

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
        eventBus.Subscribe<OnUpdateCoins>((Action<OnUpdateCoins>)HandleCoinValueChange);
        eventBus.Subscribe<OnUpdateTimer>((Action<OnUpdateTimer>)HandleTimerChanged);
        eventBus.Subscribe<OnUpdateHp>((Action<OnUpdateHp>)HandleHealthChanged);
    }

    private void HandleTimerChanged(OnUpdateTimer data)
    {
        gameplayTimerText.text = GetTimeFormat(data.value);
    }

    private void HandleHealthChanged(OnUpdateHp data)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(false);

            if (i < data.maxHp)
            {
                heartImages[i].gameObject.SetActive(true);
                heartImages[i].alpha = i < data.hp ? 1f : lostHealthAlpha;
            }
        }
    }

    private string GetTimeFormat(float amount)
    {
        TimeSpan time = TimeSpan.FromSeconds(amount);
        return time.ToString(@"mm\:ss");
    }

    private void HandleCoinValueChange(OnUpdateCoins data)
    {
        gameplayCoinAmountText.text = data.newAmount.ToString();
    }

    private void HandleGameWin(OnGameEnded data)
    {
        UiUtils.SetCanvasActive(resultScreenCanvasGroup, true);

        if (gameManager.IsCurrentLastLevel())
        {
            UiUtils.SetCanvasActive(continueButtonCG, false);
            continueButtonCG.alpha = disabledContinueAlpha;
        }

        resultCoinAmountText.text = data.moneyAmount.ToString();
        resultTimerText.text = GetTimeFormat(data.timeLeft);

        Debug.Log("Level won");
        gameManager.SetCurrentLevelWon();
        gameManager.SetPause(true);
    }

    private void HandleResultMainMenu()
    {
        gameManager.GameplayMusic.Stop();
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
        gameManager.GameplayMusic.Stop();
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
        UiUtils.SetCanvasActive(pauseButtonCG, true);
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
        eventBus.Unsubscribe<OnPickCoin>((Action<OnUpdateCoins>)HandleCoinValueChange);
        eventBus.Unsubscribe<OnUpdateHp>((Action<OnUpdateHp>)HandleHealthChanged);
    }
}