using System;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private List<IRestartable> entities = new List<IRestartable>();
    private EventBus eventBus;

    private void Awake()
    {
        eventBus = ServiceLocator.Instance.GetService<EventBus>();
        eventBus.Subscribe<OnRetry>((Action)HandleRetry);
    }

    private void Start()
    {
        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            if (monoBehaviour is IRestartable restartable)
            {
                entities.Add(restartable);
            }
        }
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
