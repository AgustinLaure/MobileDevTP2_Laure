using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private int maxLevelReached;

    [SerializeField] private float fadeTime;

    [SerializeField] public AudioMixer audioMixer;

    private HealthPoints playerHealth;

    private List<GameObject> savedLogs = new List<GameObject>();

    private int currentLevelIndex = 0;

    private int moneyAmount = 0;

    private bool isPaused = false;
    private bool isMuted = false;

    public string TargetScene { get; set; }

    private const string mainMenuSceneName = "MainMenu";
    private const string gameplaySceneName = "Gameplay";
    private const string loadingSceneName = "LoadingScreen";

    private const string maxLevelReachKey = "MaxLevelWon";
    private const string moneyKey = "Money";
    private const string maxHealthKey = "MaxHealth";

    private const string musicVolumeName = "MusicVolume";
    private const string sfxVolumeName = "SfxVolume";

    public string GetMainMenuSceneName { get { return mainMenuSceneName; } }

    public string GetGameplaySceneName { get { return gameplaySceneName; } }

    public int GetCurrentLevelIndex { get { return currentLevelIndex; } }

    public bool GetIsPaused { get { return isPaused; } }

    public int GetMaxLevelReached { get { return maxLevelReached; } }

    public List<GameObject> GetSavedLogs { get { return savedLogs; } }

    public int GetCurrentMoney { get { return moneyAmount; } }

    public bool GetIsMuted { get { return isMuted; } }

    public HealthPoints GetPlayerHealthPoints { get { return playerHealth; } }

    [field: SerializeField] public Image Fade { get; set; }

    [field: SerializeField] public AudioSource GameplayMusic { get; set; }
    [field: SerializeField] public AudioSource MenuMusic { get; set; }
    [field: SerializeField] public AudioSource ButtonSound { get; set; }

    public float GetFadeTime { get { return fadeTime; } }

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

    public void SetVolume(float volume, string mixer)
    {

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

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioMixer.SetFloat(musicVolumeName, isMuted ? -80f : 0f);
        audioMixer.SetFloat(sfxVolumeName, isMuted ? -80f : 0f);
    }

    public void AddMaxHealth(int amount)
    {
        playerHealth.maxHp += amount;
        PlayerPrefs.SetInt(maxHealthKey, playerHealth.maxHp);
    }

    public void SetPause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        this.isPaused = isPaused;
    }

    public void BuyHealth(int healthAdd, int moneySubtract)
    {
        AddMaxHealth(healthAdd);
        moneyAmount -= moneySubtract;

        PlayerPrefs.SetInt(moneyKey, moneyAmount);
    }

    private void UpdateLevelPrefs()
    {
        PlayerPrefs.SetInt(maxLevelReachKey, maxLevelReached);
    }

    public void SetCurrentLevelWon()
    {
        if (maxLevelReached <= currentLevelIndex)
        {
            maxLevelReached = currentLevelIndex + 1;

            if (maxLevelReached > levelPrefabs.Length)
            {
                maxLevelReached = levelPrefabs.Length - 1;
            }

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
        SetPause(false);
        currentLevelIndex = index;
        LoadScene(gameplaySceneName);
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return FadeOut();
        yield return SceneManager.LoadSceneAsync(loadingSceneName);
    }

    public IEnumerator FadeOut()
    {
        Fade.gameObject.SetActive(true);

        float t = Fade.color.a;
        Color currentColor = Fade.color;

        while (t < 1f)
        {
            t += Mathf.Min(Time.unscaledDeltaTime, 0.05f) / fadeTime;
            currentColor.a = Mathf.Clamp01(t);
            Fade.color = currentColor;
            yield return null;
        }

        currentColor.a = 1f;
        Fade.color = currentColor;
    }

    public void LoadScene(string sceneName)
    {
        TargetScene = sceneName;

        StartCoroutine(LoadSceneCoroutine());
    }
}
