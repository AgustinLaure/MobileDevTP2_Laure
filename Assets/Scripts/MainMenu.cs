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

    [SerializeField] private Button levelSelectLevel1Button;
    [SerializeField] private Button levelSelectLevel2Button;
    [SerializeField] private Button levelSelectLevel3Button;
    [SerializeField] private Button levelSelectBackButton;

    [SerializeField] private Button creditsBackButton;

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

        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();
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
        //load logs scene
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
    }
}
