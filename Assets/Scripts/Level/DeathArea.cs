using System.Collections;
using UnityEngine;

public class DeathArea : MonoBehaviour, IRestartable
{
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private Color blinkingColor;
    [SerializeField] private Color activeColor;

    [SerializeField] private AreaTrigger areaTrigger;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private LevelData levelData;

    private bool isActive = false;
    private bool hasHitPlayerThisBlink = false;

    private EventBus eventBus;

    private Coroutine blinkCoroutine = null;


    private WaitForSeconds blinkCooldownTimer;
    private WaitForSeconds blinkTimer;

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;
        eventBus = serviceLocator.GetService<EventBus>();

        areaTrigger.OnTriggerEntered += HandleCollision;
    }

    public void Init(LevelData data)
    {
        levelData = data;

        blinkTimer = new WaitForSeconds(levelData.DeathAreaBlinkTime);
        blinkCooldownTimer = new WaitForSeconds(levelData.DeathAreaBlinkCooldown);
    }

    private void Update()
    {
        if (blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkCoroutine());
        }
    }

    private IEnumerator BlinkCoroutine()
    {
        hasHitPlayerThisBlink = false;

        float t = 0f;
        Color startColor = blinkingColor;
        boxCollider.enabled = false;
        isActive = false;

        while (t < levelData.DeathAreaFadeInTime)
        {
            t += Time.deltaTime;
            startColor.a = fadeCurve.Evaluate(t / levelData.DeathAreaFadeInTime);
            spriteRenderer.color = startColor;

            yield return null;
        }

        spriteRenderer.color = activeColor;

        isActive = true;
        boxCollider.enabled = true;

        yield return blinkTimer;

        boxCollider.enabled = false;
        isActive = false;

        startColor.a = 0f;
        spriteRenderer.color = startColor;

        yield return blinkCooldownTimer;

        blinkCoroutine = null;
    }

    private void HandleCollision(Collider2D collider)
    {
        if (collider.CompareTag(Player.GetPlayerTag) && isActive && !hasHitPlayerThisBlink)
        {
            eventBus.Raise<OnHitPlayer>(levelData.DeathAreaDamage);
            hasHitPlayerThisBlink = true;
        }
    }

    public void Restart(Transform position)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        areaTrigger.OnTriggerEntered -= HandleCollision;
    }
}
