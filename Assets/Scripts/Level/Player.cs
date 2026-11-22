using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;


public class Player : MonoBehaviour, IRestartable
{
    [Header("References")]
    [SerializeField] private AreaCollider areaTrigger;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Grid grid;
    private GameManager gameManager;

    [Header("Configs")]
    [SerializeField] private float speed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private BoxCollider2D wallCheck;
    [SerializeField] private float wallCheckDistance;

    private Vector3 startPos = Vector3.zero;

    private int hp = 1;
    private int maxHp = 1;

    private InputSystemController inputController;
    private EventBus eventBus;

    private bool isTraveling = false;
    private Vector3 positionAsVec3 = new Vector3();

    private string wallTag = "Wall";

    private ForceMode2D pushForceMode = ForceMode2D.Impulse;

    private Coroutine dashCooldownCoroutine = null;
    private WaitForSeconds dashCooldownTimer;


    private const string playerTag = "Player";

    public static string GetPlayerTag { get { return playerTag; } }

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();

        eventBus = serviceLocator.GetService<EventBus>();
        inputController = serviceLocator.GetService<InputSystemController>();

        eventBus.Subscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        eventBus.Subscribe<OnHitPlayer>((Action<OnHitPlayer>)HandleHit);
        areaTrigger.OnColliderEntered += HandleWallCollision;

        dashCooldownTimer = new WaitForSeconds(dashCooldown);

        startPos = transform.position;

        maxHp = gameManager.GetPlayerHealthPoints.maxHp;
        hp = maxHp;
        eventBus.Raise<OnUpdateHp>(maxHp, hp);
    }

    private void Start()
    {
        SnapToGrid();
    }

    public void Restart(Transform position)
    {
        isTraveling = false;
        rb.linearVelocity = Vector2.zero;
        transform.position = startPos;
        hp = maxHp;
        eventBus.Raise<OnUpdateHp>(maxHp, hp);
    }

    private void HandleHit(OnHitPlayer data)
    {
        hp -= data.damage;
        eventBus.Raise<OnUpdateHp>(maxHp, hp);

        if (hp <= 0)
        {
            hp = 0;
            eventBus.Raise<OnRetry>();
        }
    }

    private void HandleSwipe(OnSwipe data)
    {
        if (!isTraveling && !gameManager.GetIsPaused)
        {
            Launch(data.direction);
        }
    }

    private IEnumerator DashCooldownCoroutine()
    {
        yield return dashCooldownTimer;

        isTraveling = false;
    }

    private void SnapToGrid()
    {
        Vector3Int cellPosition = grid.WorldToCell(transform.position);

        Vector3 cellCenter = grid.GetCellCenterWorld(cellPosition);
        cellCenter.z = transform.position.z;

        transform.position = cellCenter;
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
            SnapToGrid();
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube((Vector2)wallCheck.bounds.center + new Vector2(0f, 1f) * wallCheckDistance, wallCheck.bounds.size);
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        areaTrigger.OnColliderEntered -= HandleWallCollision;
    }
}
