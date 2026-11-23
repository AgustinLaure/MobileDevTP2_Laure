using System;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private AudioSource restartSound;
    [SerializeField] private AudioSource winSound;

    private IRestartable[] entities;
    private EventBus eventBus;
    private GameManager gameManager;

    private int coinAmount = 0;
    private float timer = 0f;

    private LevelData currentLevelData = null;

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
        gameManager.SetPause(false);

        if (!gameManager.GameplayMusic.isPlaying)
        {
            gameManager.GameplayMusic.Play();
        }
    }

    public void Init(LevelData levelData, IRestartable[] entities)
    {
        currentLevelData = levelData;
        timer = currentLevelData.LevelMaxTime;
        this.entities = entities;
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
        Debug.Log("CoinPicked");
        coinAmount += data.value;
        eventBus.Raise<OnUpdateCoins>(coinAmount);
    }

    private void HandleRetry()
    {
        foreach (IRestartable entity in entities)
        {
            entity.Restart();
        }

        Debug.Log("LevelRetry");
        timer = currentLevelData.LevelMaxTime;
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
