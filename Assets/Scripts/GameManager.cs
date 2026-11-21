using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private int maxLevelReached;

    private HealthPoints playerHealth;

    private List<GameObject> savedLogs = new List<GameObject>();

    private int currentLevelIndex = -1;

    private int moneyAmount = 0;

    private bool isPaused = false;

    private const string mainMenuSceneName = "MainMenu";
    private const string gameplaySceneName = "Gameplay";

    private const string maxLevelReachKey = "MaxLevelWon";
    private const string moneyKey = "Money";
    private const string maxHealthKey = "MaxHealth";

    public string GetMainMenuSceneName { get { return mainMenuSceneName; } }

    public string GetGameplaySceneName { get { return gameplaySceneName; } }

    public int GetCurrentLevelIndex { get { return currentLevelIndex; } }

    public bool GetIsPaused { get { return isPaused; } }

    public int GetMaxLevelReached { get { return maxLevelReached; } }

    public List<GameObject> GetSavedLogs { get { return savedLogs; } }

    public int GetCurrentMoney { get { return moneyAmount; } }

    public HealthPoints GetPlayerHealthPoints { get { return playerHealth; } }

    private void Awake()
    {
        maxLevelReached = PlayerPrefs.GetInt(maxLevelReachKey, maxLevelReached);
        moneyAmount = PlayerPrefs.GetInt(moneyKey, moneyAmount);

        playerHealth.maxHp = PlayerPrefs.GetInt(maxHealthKey, playerHealth.maxHp);
        if (playerHealth.maxHp == 0)
        {
            playerHealth.maxHp = 1;
        }
        playerHealth.hp = playerHealth.maxHp;
    }

    public void AddGameLog(GameObject log)
    {
        savedLogs.Add(log);
    }

    public void DeleteGameLogs()
    {
        foreach (GameObject log in savedLogs)
        {
            if (log != null)
            {
                Destroy(log);
            }
        }

        savedLogs.Clear();
    }

    public void SetPause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        this.isPaused = isPaused;
    }

    private void UpdateHealthPrefs()
    {
        PlayerPrefs.SetInt(maxHealthKey, playerHealth.maxHp);
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

    public void AddMoney(int amount)
    {
        moneyAmount += amount;

        PlayerPrefs.SetInt(moneyKey, moneyAmount);
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
