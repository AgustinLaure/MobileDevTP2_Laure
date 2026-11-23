using UnityEngine;

public class Coin : MonoBehaviour, IRestartable
{
    [SerializeField] private GameObject body;
    [SerializeField] private AreaTrigger areaTrigger;

    private LevelData levelData;

    private EventBus eventBus;
    private GameManager gameManager;

    private const float coinVolume = 0.6f;

    private void Awake()
    {
        areaTrigger.OnTriggerEntered += HandleCollider;

        ServiceLocator serviceLocator = ServiceLocator.Instance;

        eventBus = serviceLocator.GetService<EventBus>();
        gameManager = serviceLocator.GetService<GameManager>();
    }

    public void Init(LevelData data)
    {
        levelData = data;
    }

    private void HandleCollider(Collider2D collider)
    {
        if (collider.CompareTag(Player.GetPlayerTag))
        {
            eventBus.Raise<OnPickCoin>(levelData.CoinValue);
            gameManager.PlaySfx(levelData.CoinPickAudio, coinVolume);
            body.SetActive(false);
        }
    }

    public void Restart(Transform position)
    {
        body.SetActive(true);
    }

    private void OnDestroy()
    {
        areaTrigger.OnTriggerEntered -= HandleCollider;
    }
}
