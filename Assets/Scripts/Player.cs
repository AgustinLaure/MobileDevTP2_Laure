using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AreaCollider areaCollider;
    [SerializeField] private Rigidbody2D rb;

    [Header("Configs")]
    [SerializeField] private float speed;

    private InputSystemController inputController;
    private EventBus eventBus;

    private bool isTraveling = false;
    private Vector3 positionAsVec3 = new Vector3();

    private string wallTag = "Wall";

    private ForceMode2D pushForceMode = ForceMode2D.Impulse;

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        eventBus = serviceLocator.GetService<EventBus>();
        inputController = serviceLocator.GetService<InputSystemController>();

        eventBus.Subscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        areaCollider.OnColliderEntered += HandleWallCollision;

    }

    private void HandleSwipe(OnSwipe data)
    {
        if (!isTraveling)
        {
            Launch(data.direction);
        }
    }

    private void Launch(Vector2 direction)
    {
        rb.AddForce(direction * speed, pushForceMode);
        isTraveling = true;
    }

    private void HandleWallCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            isTraveling = false;
        }
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        areaCollider.OnColliderEntered -= HandleWallCollision;
    }
}
