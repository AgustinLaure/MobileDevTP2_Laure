using JetBrains.Annotations;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private int maxLevelReached;

    private int currentLevelIndex = -1;

    private bool isPaused = false;

    private const string mainMenuSceneName = "MainMenu";
    private const string gameplaySceneName = "Gameplay";

    private const string maxLevelReachKey = "MaxLevelWon";

    public string GetMainMenuSceneName { get { return mainMenuSceneName; } }

    public string GetGameplaySceneName { get { return gameplaySceneName; } }

    public int GetCurrentLevelIndex { get { return currentLevelIndex; } }

    public bool GetIsPaused { get { return isPaused; } }

    public int GetMaxLevelReached { get { return maxLevelReached; } }

    private void Awake()
    {
        maxLevelReached = PlayerPrefs.GetInt(maxLevelReachKey, maxLevelReached);
    }

    public void SetPause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        this.isPaused = isPaused;
    }

    private void UpdateLevelPrefs()
    {
        PlayerPrefs.SetInt(maxLevelReachKey, maxLevelReached);
    }

    public void SetCurrentLevelWon()
    {
        if (maxLevelReached < currentLevelIndex)
        {
            maxLevelReached = currentLevelIndex;
            UpdateLevelPrefs();
        }
    }

    public void SetLevelWon(int index, bool state)
    {
        if (index >= 0 && index < levelPrefabs.Length)
        {
            maxLevelReached = currentLevelIndex;
        }
        UpdateLevelPrefs();
    }

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
