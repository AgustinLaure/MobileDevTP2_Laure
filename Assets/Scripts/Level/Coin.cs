using UnityEngine;

public class Coin : MonoBehaviour, IRestartable
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private GameObject body;
    [SerializeField] private AreaTrigger areaTrigger;
    [SerializeField] private int value;
    private EventBus eventBus;
    private void Awake()
    {
        areaTrigger.OnTriggerEntered += HandleCollider;

        eventBus = ServiceLocator.Instance.GetService<EventBus>();
    }

    private void HandleCollider(Collider2D collider)
    {
        if (collider.CompareTag(Player.GetPlayerTag))
        {
            eventBus.Raise<OnPickCoin>(value);
            sound.Play();
            body.SetActive(false);
        }
    }

    public void Restart(Transform position)
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        areaTrigger.OnTriggerEntered -= HandleCollider;
    }
}
