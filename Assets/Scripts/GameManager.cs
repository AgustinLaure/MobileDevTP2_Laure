using JetBrains.Annotations;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;

    private int currentLevelIndex = -1;

    private const string mainMenuSceneName = "MainMenu";
    private const string gameplaySceneName = "Gameplay";

    public string GetMainMenuSceneName { get { return mainMenuSceneName; } }

    public string GetGameplaySceneName { get { return gameplaySceneName; } }

    public int GetCurrentLevelIndex { get { return currentLevelIndex; } }


    public bool GetCurrentLevelPrefab(out GameObject levelPrefab)
    {
        if (currentLevelIndex >= 0 && currentLevelIndex < levelPrefabs.Length)
        {
            levelPrefab = levelPrefabs[currentLevelIndex];
            return true;
        }
        else
        {
            levelPrefab = null;
            return false;
        }
    }

    public bool GetLevelPrefab(int index, out GameObject levelPrefab)
    {
        if (GetCurrentLevelPrefab(out GameObject prefab))
        {
            levelPrefab = prefab;
            return true;
        }
        else
        {
            levelPrefab = null;
        }

        return false;
    }

    public bool IsCurrentLastLevel()
    {
        return currentLevelIndex + 1 >= levelPrefabs.Length;
    }

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
