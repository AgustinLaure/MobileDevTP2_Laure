using JetBrains.Annotations;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;

    private int currentLevel = -1;

    private const string mainMenuSceneName = "MainMenu";
    private const string gameplaySceneName = "Gameplay";

    public string GetMainMenuSceneName { get { return mainMenuSceneName; } }

    public string GetGameplaySceneName { get { return gameplaySceneName; } }

    public bool GetCurrentLevelPrefab(out GameObject levelPrefab)
    {
        if (currentLevel > 0 && currentLevel < levelPrefabs.Length)
        {
            levelPrefab = levelPrefabs[currentLevel];
            return true;
        }
        else
        {
            levelPrefab = null;
            return false;
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(gameplaySceneName);
        currentLevel = index;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
