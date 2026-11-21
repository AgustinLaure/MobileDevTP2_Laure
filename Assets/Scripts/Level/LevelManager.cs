using System;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private List<IRestartable> entities = new List<IRestartable>();
    private EventBus eventBus;
    private GameManager gameManager;

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        eventBus = serviceLocator.GetService<EventBus>();
        eventBus.Subscribe<OnRetry>((Action)HandleRetry);

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

        gameManager.SetPause(false);
    }

    private void HandleRetry()
    {
        foreach (IRestartable entity in entities)
        {
            entity.Restart();
        }
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnRetry>((Action)HandleRetry);
    }
}
