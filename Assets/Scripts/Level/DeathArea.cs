using System.Collections;
using UnityEngine;

public class DeathArea : MonoBehaviour, IRestartable
{
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private AreaTrigger areaTrigger;
    [SerializeField] private int damage;
    [SerializeField] private float blinkCooldown;
    [SerializeField] private float blinkTime;

    [SerializeField] private Color blinkingColor;
    [SerializeField] private Color activeColor;

    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private float fadeInTime;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isActive = false;

    private EventBus eventBus;

    private Coroutine blinkCoroutine = null;

    private WaitForSeconds blinkCooldownTimer;
    private WaitForSeconds blinkTimer;

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;
        eventBus = serviceLocator.GetService<EventBus>();

        areaTrigger.OnTriggerEntered += HandleCollision;

        blinkTimer = new WaitForSeconds(blinkTime);
        blinkCooldownTimer = new WaitForSeconds(blinkCooldown);
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
        float t = 0f;
        Color startColor = blinkingColor;
        boxCollider.enabled = false;
        isActive = false;

        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            startColor.a = fadeCurve.Evaluate(t / fadeInTime);
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
        if (collider.CompareTag(Player.GetPlayerTag) && isActive)
        {
            eventBus.Raise<OnHitPlayer>(damage);
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
