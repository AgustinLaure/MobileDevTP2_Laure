using System;
using System.Collections;
using Unity.Properties;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AreaCollider areaTrigger;
    [SerializeField] private Rigidbody2D rb;

    [Header("Configs")]
    [SerializeField] private float speed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private BoxCollider2D wallCheck;
    [SerializeField] private float wallCheckDistance;

    private InputSystemController inputController;
    private EventBus eventBus;

    private bool isTraveling = false;
    private Vector3 positionAsVec3 = new Vector3();

    private string wallTag = "Wall";

    private ForceMode2D pushForceMode = ForceMode2D.Impulse;

    private Coroutine dashCooldownCoroutine = null;
    private WaitForSeconds dashCooldownTimer;

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        eventBus = serviceLocator.GetService<EventBus>();
        inputController = serviceLocator.GetService<InputSystemController>();

        eventBus.Subscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        areaTrigger.OnColliderEntered += HandleWallCollision;

        dashCooldownTimer = new WaitForSeconds(dashCooldown);
    }

    private void HandleSwipe(OnSwipe data)
    {
        if (!isTraveling)
        {
            Launch(data.direction);
        }
    }

    private IEnumerator DashCooldownCoroutine()
    {
        yield return dashCooldownTimer;

        isTraveling = false;
    }

    private void Launch(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(
        (Vector2)wallCheck.bounds.center + direction * wallCheckDistance,
        wallCheck.bounds.size,
        0f,
        direction.normalized,
        wallCheckDistance,
        wallLayer
    );

        if (hit.collider == null)
        {
            rb.AddForce(direction * speed, pushForceMode);
            isTraveling = true;
        }
    }

    private void HandleWallCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            rb.linearVelocity = Vector2.zero;

            if (dashCooldownCoroutine != null)
            {
                StopCoroutine(dashCooldownCoroutine);
            }

            dashCooldownCoroutine = StartCoroutine(DashCooldownCoroutine());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube((Vector2)wallCheck.bounds.center + new Vector2(0f, 1f) * wallCheckDistance, wallCheck.bounds.size);
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        areaTrigger.OnColliderEntered -= HandleWallCollision;
    }
}
