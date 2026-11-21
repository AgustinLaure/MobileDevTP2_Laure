using UnityEngine;

public class WinDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AreaTrigger areaTrigger;
    private EventBus eventBus;

    private void Awake()
    {
        areaTrigger.OnTriggerEntered += HandleTrigger;

        eventBus = ServiceLocator.Instance.GetService<EventBus>();
    }

    private void HandleTrigger(Collider2D collider)
    {
        if (collider.CompareTag(Player.GetPlayerTag))
        {
            eventBus.Raise<OnGameEndRequest>(false);
        }
    }

    private void OnDestroy()
    {
        areaTrigger.OnTriggerEntered -= HandleTrigger;
    }
}
