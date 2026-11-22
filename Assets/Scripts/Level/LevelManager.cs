using System;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private AudioSource restartSound;
    [SerializeField] private AudioSource winSound;

    private List<IRestartable> entities = new List<IRestartable>();
    private EventBus eventBus;
    private GameManager gameManager;

    private int coinAmount = 0;
    private float timer = 0f;

    private LevelData currentLevelData = null;
    public int GetCoinAmount { get { return coinAmount; } }

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        eventBus = serviceLocator.GetService<EventBus>();
        eventBus.Subscribe<OnRetry>((Action)HandleRetry);
        eventBus.Subscribe<OnPickCoin>((Action<OnPickCoin>)HandlePickCoin);
        eventBus.Subscribe<OnGameEndRequest>((Action<OnGameEndRequest>)HandleEndRequest);

        gameManager = serviceLocator.GetService<GameManager>();
    }

    private void Start()
    {
        GameObject levelPrefab = null;

        if (gameManager.GetCurrentLevelPrefab(out levelPrefab))
        {
            Instantiate(levelPrefab, null, true);
        }

        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            if (monoBehaviour is IRestartable restartable)
            {
                entities.Add(restartable);
            }
        }

        currentLevelData = FindAnyObjectByType<Level>().GetLevelData;

        timer = currentLevelData.maxTime;

        gameManager.SetPause(false);

        if (!gameManager.GameplayMusic.isPlaying)
        {
            gameManager.GameplayMusic.Play();
        }
    }

    private void HandleEndRequest(OnGameEndRequest data)
    {
        eventBus.Raise<OnGameEnded>(data.hasWon, timer, coinAmount);
        winSound.Play();
        gameManager.AddMoney(coinAmount);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        eventBus.Raise<OnUpdateTimer>(timer);

        if (timer <= 0f)
        {
            HandleRetry();
        }
    }
    private void HandlePickCoin(OnPickCoin data)
    {
        coinAmount += data.value;
        eventBus.Raise<OnUpdateCoins>(coinAmount);
    }


    private void HandleRetry()
    {
        foreach (IRestartable entity in entities)
        {
            entity.Restart();
        }

        timer = currentLevelData.maxTime;
        coinAmount = 0;
        eventBus.Raise<OnUpdateCoins>(coinAmount);
        restartSound.Play();
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnRetry>((Action)HandleRetry);
        eventBus.Unsubscribe<OnPickCoin>((Action<OnPickCoin>)HandlePickCoin);
        eventBus.Unsubscribe<OnGameEndRequest>((Action<OnGameEndRequest>)HandleEndRequest);
    }
}
