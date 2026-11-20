using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private EventBus eventBus;

    private void Awake()
    {
        eventBus = ServiceLocator.Instance.GetService<EventBus>();

        eventBus.Subscribe<OnGameEnded>((Action<OnGameEnded>)HandleGameEnd);
    }

    private void HandleGameEnd(OnGameEnded data)
    {
        Debug.Log("TERMINO");
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnGameEnded>((Action<OnGameEnded>)HandleGameEnd);
    }
}
