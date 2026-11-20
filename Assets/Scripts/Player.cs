using System;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum WallDetection
    {
        Top,
        Down,
        Left,
        Right
    }

    [Header("References")]
    [SerializeField] private AreaTrigger areaTrigger;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject[] wallDetections;

    [Header("Configs")]
    [SerializeField] private float speed;

    private InputSystemController inputController;
    private EventBus eventBus;

    private bool isTraveling = false;
    private Vector3 positionAsVec3 = new Vector3();

    private string wallTag = "Wall";

    private ForceMode2D pushForceMode = ForceMode2D.Impulse;

    private const float epsilon = 0.1f;

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        eventBus = serviceLocator.GetService<EventBus>();
        inputController = serviceLocator.GetService<InputSystemController>();

        eventBus.Subscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        areaTrigger.OnTriggerEntered += HandleWallCollision;
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

        DisableWallDetection();

        int detectionIndex = 0;
        if (direction.x > 0f)
        {
            detectionIndex = 3;
        }
        else if (direction.x < 0f)
        {
            detectionIndex = 2;
        }
        else if (direction.y > 0f)
        {
            detectionIndex = 0;
        }
        else if (direction.y < 0f)
        {
            detectionIndex = 1;
        }
        wallDetections[detectionIndex].SetActive(true);

        isTraveling = true;
    }

    private void DisableWallDetection()
    {
        foreach (GameObject wallDetection in wallDetections)
        {
            wallDetection.SetActive(false);
        }
    }

    private void HandleWallCollision(Collider2D collider)
    {
        if (collider.CompareTag(wallTag))
        {
            isTraveling = false;
        }
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnSwipe>((Action<OnSwipe>)HandleSwipe);
        areaTrigger.OnTriggerEntered -= HandleWallCollision;
    }
}
