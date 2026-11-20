using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string mainMenuSceneName = "MainMenu";
    private const string gameplaySceneName = "Gameplay";

    public string GetMainMenuSceneName { get { return mainMenuSceneName; } }

    public string GetGameplaySceneName { get { return gameplaySceneName; } }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
