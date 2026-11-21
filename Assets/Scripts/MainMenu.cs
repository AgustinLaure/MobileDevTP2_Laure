using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button mainMenuPlayButton;
    [SerializeField] private Button mainMenuCreditsButton;
    [SerializeField] private Button mainMenuLogsButton;
    [SerializeField] private Button mainMenuMuteButton;

    [SerializeField] private CanvasGroup mainMenuGC;
    [SerializeField] private CanvasGroup levelSelectGC;
    [SerializeField] private CanvasGroup creditsGC;
    [SerializeField] private CanvasGroup logsGC;

    [SerializeField] private Button levelSelectLevel1Button;
    [SerializeField] private Button levelSelectLevel2Button;
    [SerializeField] private Button levelSelectLevel3Button;
    [SerializeField] private Button levelSelectBackButton;

    [SerializeField] private Button logBackButton;

    [SerializeField] private CanvasGroup[] levelSelectCG;

    [SerializeField] private Button creditsBackButton;

    [SerializeField] private float nonSelectableLevelAlpha;

    [SerializeField] private TMPro.TMP_Text moneyText;

    private GameManager gameManager;

    private void Awake()
    {
        mainMenuPlayButton.onClick.AddListener(HandleMainMenuPlay);
        mainMenuCreditsButton.onClick.AddListener(HandleMainMenuCredits);
        mainMenuLogsButton.onClick.AddListener(HandleMainMenuLogs);
        mainMenuMuteButton.onClick.AddListener(HandleMainMenuMute);

        levelSelectLevel1Button.onClick.AddListener(HandleLevelSelectLevel1);
        levelSelectLevel2Button.onClick.AddListener(HandleLevelSelectLevel2);
        levelSelectLevel3Button.onClick.AddListener(HandleLevelSelectLevel3);
        levelSelectBackButton.onClick.AddListener(HandleLevelSelectBack);

        creditsBackButton.onClick.AddListener(HandleCreditsBack);

        logBackButton.onClick.AddListener(HandleLogBack);

        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();

        gameManager.SetPause(false);

        int maxLevelReached = gameManager.GetMaxLevelReached;

        for (int i = 0; i < levelSelectCG.Length; i++)
        {
            UiUtils.SetCanvasActive(levelSelectCG[i], false);
            levelSelectCG[i].alpha = nonSelectableLevelAlpha;
        }

        for (int i = 0; i <= maxLevelReached + 1; i++)
        {
            if (i < levelSelectCG.Length)
            {
                UiUtils.SetCanvasActive(levelSelectCG[i], true);
            }
        }
    }

    private void Start()
    {
        moneyText.text = gameManager.GetCurrentMoney.ToString();
    }

    private void HandleMainMenuPlay()
    {
        UiUtils.SetCanvasActive(levelSelectGC, true);
    }

    private void HandleMainMenuCredits()
    {
        UiUtils.SetCanvasActive(mainMenuGC, false);
        UiUtils.SetCanvasActive(creditsGC, true);
    }

    private void HandleMainMenuLogs()
    {
        UiUtils.SetCanvasActive(mainMenuGC, false);
        UiUtils.SetCanvasActive(logsGC, true);
    }

    private void HandleMainMenuMute()
    {
        //mute functionality
    }

    private void HandleLevelSelectLevel1()
    {
        gameManager.LoadLevel(0);
    }

    private void HandleLevelSelectLevel2()
    {
        gameManager.LoadLevel(1);
    }

    private void HandleLevelSelectLevel3()
    {
        gameManager.LoadLevel(2);
    }

    private void HandleLevelSelectBack()
    {
        UiUtils.SetCanvasActive(levelSelectGC, false);
    }

    private void HandleCreditsBack()
    {
        UiUtils.SetCanvasActive(creditsGC, false);
        UiUtils.SetCanvasActive(mainMenuGC, true);
    }

    private void HandleLogBack()
    {
        UiUtils.SetCanvasActive(logsGC, false);
        UiUtils.SetCanvasActive(mainMenuGC, true);
    }

    private void OnDestroy()
    {
        mainMenuPlayButton.onClick.RemoveListener(HandleMainMenuPlay);
        mainMenuCreditsButton.onClick.RemoveListener(HandleMainMenuCredits);
        mainMenuLogsButton.onClick.RemoveListener(HandleMainMenuLogs);
        mainMenuMuteButton.onClick.RemoveListener(HandleMainMenuMute);

        levelSelectLevel1Button.onClick.RemoveListener(HandleLevelSelectLevel1);
        levelSelectLevel2Button.onClick.RemoveListener(HandleLevelSelectLevel2);
        levelSelectLevel3Button.onClick.RemoveListener(HandleLevelSelectLevel3);
        levelSelectBackButton.onClick.RemoveListener(HandleLevelSelectBack);

        creditsBackButton.onClick.RemoveListener(HandleCreditsBack);

        logBackButton.onClick.RemoveListener(HandleLogBack);
    }
}
