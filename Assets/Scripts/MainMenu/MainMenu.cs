using JetBrains.Annotations;
using System;
using System.ComponentModel;
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

    [SerializeField] private Button shopButton;
    [SerializeField] private Button shopBackButton;

    [SerializeField] private CanvasGroup shopCG;

    [SerializeField] private CanvasGroup[] levelSelectCG;

    [SerializeField] private Button creditsBackButton;

    [SerializeField] private float nonSelectableLevelAlpha;

    [SerializeField] private TMPro.TMP_Text moneyText;

    [SerializeField] private Image muteImage;

    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;

    private GameManager gameManager;
    private EventBus eventBus;

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

        shopButton.onClick.AddListener(HandleShopButton);
        shopBackButton.onClick.AddListener(HandleShopBackButton);

        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();
        eventBus = serviceLocator.GetService<EventBus>();

        eventBus.Subscribe<OnMainMenuUpdateMoney>((Action)HandleUpdateMoney);

        gameManager.SetPause(false);

        int maxLevelReached = gameManager.GetMaxLevelReached;

        for (int i = 0; i < levelSelectCG.Length; i++)
        {
            UiUtils.SetCanvasActive(levelSelectCG[i], false);
            levelSelectCG[i].alpha = nonSelectableLevelAlpha;
        }

        for (int i = 0; i <= maxLevelReached; i++)
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

    private void HandleShopButton()
    {
        UiUtils.SetCanvasActive(shopCG, true);
    }

    private void HandleShopBackButton()
    {
        UiUtils.SetCanvasActive(shopCG, false);
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

    private void HandleUpdateMoney()
    {
        moneyText.text = gameManager.GetCurrentMoney.ToString();
    }

    private void HandleMainMenuMute()
    {
        gameManager.ToggleMute();
        muteImage.sprite = gameManager.GetIsMuted ? mutedSprite : unmutedSprite;
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

        shopButton.onClick.RemoveListener(HandleShopButton);
        shopBackButton.onClick.RemoveListener(HandleShopBackButton);

        eventBus.Unsubscribe<OnMainMenuUpdateMoney>((Action)HandleUpdateMoney);
    }
}
