using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private InputActionReference tapAction;

    [SerializeField] private float gyroStartAngle;
    [SerializeField] private float gyroEndAngle;
    [SerializeField] private Transform gyro;

    [SerializeField] private Transform swipeStartPos;
    [SerializeField] private Transform swipeEndPos;
    [SerializeField] private Transform swipe;

    [SerializeField] private AnimationCurve gyroAnimation;
    [SerializeField] private float gyroTime;
    [SerializeField] private float gyroLoopTime;

    [SerializeField] private AnimationCurve swipeAnimation;
    [SerializeField] private float swipeTime;
    [SerializeField] private float swipeLoopTime;

    [SerializeField] private float fadeTime;
    [SerializeField] private CanvasGroup firstScreenCanvasGroup;
    [SerializeField] private CanvasGroup secondScreenCanvasGroup;
    [SerializeField] private CanvasGroup tutorialCanvasGroup;
    [SerializeField] private CanvasGroup backgroundCanvasGroup;

    [SerializeField] private float tutorialTime;

    private GameManager gameManager;

    private WaitForSecondsRealtime swipeLoopTimer;
    private WaitForSecondsRealtime gyroLoopTimer;

    private Coroutine tutorialCoroutine = null;
    private Coroutine swipeAnimationCoroutine = null;
    private Coroutine gyroAnimationCoroutine = null;

    private bool isFirstStepCompleted = false;

    private bool hasTapped = false;

    private void Awake()
    {
        swipeLoopTimer = new WaitForSecondsRealtime(swipeLoopTime);
        gyroLoopTimer = new WaitForSecondsRealtime(gyroLoopTime);

        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();
        tapAction.action.actionMap?.Enable();
        tapAction.action.Enable();
        tapAction.action.performed += OnTap;
    }


    private void Start()
    {
        gameManager.SetPause(true);
        tutorialCoroutine = StartCoroutine(TutorialCoroutine());
    }

    private void Update()
    {
        if (!isFirstStepCompleted)
        {
            if (tutorialCoroutine != null)
            {
                if (swipeAnimationCoroutine == null)
                {
                    swipeAnimationCoroutine = StartCoroutine(SwipeAnimationCoroutine());
                }

                if (gyroAnimationCoroutine == null)
                {
                    gyroAnimationCoroutine = StartCoroutine(GyroAnimationCoroutine());
                }
            }
        }
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        hasTapped = true;
    }

    private IEnumerator TutorialCoroutine()
    {
        swipeAnimationCoroutine = StartCoroutine(SwipeAnimationCoroutine());
        gyroAnimationCoroutine = StartCoroutine(GyroAnimationCoroutine());

        while (!hasTapped)
        {
            yield return null;
        }

        hasTapped = false;

        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime / fadeTime;
            firstScreenCanvasGroup.alpha = t;

            yield return null;
        }

        firstScreenCanvasGroup.alpha = 0f;

        StopCoroutine(swipeAnimationCoroutine);
        swipeAnimationCoroutine = null;

        StopCoroutine(gyroAnimationCoroutine);
        gyroAnimationCoroutine = null;

        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeTime;
            secondScreenCanvasGroup.alpha = t;

            yield return null;
        }
        secondScreenCanvasGroup.alpha = 1f;

        while (!hasTapped)
        {
            yield return null;
        }

        t = 1f;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime / fadeTime;
            tutorialCanvasGroup.alpha = t;

            yield return null;
        }
        tutorialCanvasGroup.alpha = 0f;

        UiUtils.SetCanvasActive(backgroundCanvasGroup, false);

        firstScreenCanvasGroup.gameObject.SetActive(false);
        secondScreenCanvasGroup.gameObject.SetActive(false);

        gameManager.SetPause(false);
        tutorialCoroutine = null;
    }

    private IEnumerator SwipeAnimationCoroutine()
    {
        float t = 0f;

        Vector3 startPos = swipeStartPos.position;
        Vector3 endPos = swipeEndPos.position;

        while (t < swipeTime)
        {
            t += Time.unscaledDeltaTime;
            swipe.position = Vector3.Lerp(startPos, endPos, swipeAnimation.Evaluate(t / swipeTime));

            yield return null;
        }

        swipe.position = endPos;

        yield return swipeLoopTimer;

        t = 0f;
        while (t < swipeTime)
        {
            t += Time.unscaledDeltaTime;
            swipe.position = Vector3.Lerp(endPos, startPos, swipeAnimation.Evaluate(t / swipeTime));

            yield return null;
        }

        swipe.position = startPos;

        swipeAnimationCoroutine = null;
    }

    private IEnumerator GyroAnimationCoroutine()
    {
        float t = 0f;

        Quaternion startRotation = Quaternion.Euler(0f, 0f, gyroStartAngle);
        Quaternion endRotation = Quaternion.Euler(0f, 0f, gyroEndAngle);

        while (t < gyroTime)
        {
            t += Time.unscaledDeltaTime;
            gyro.rotation = Quaternion.Slerp(startRotation, endRotation, gyroAnimation.Evaluate(t / gyroTime));

            yield return null;
        }

        gyro.rotation = endRotation;

        yield return gyroLoopTimer;

        t = 0f;
        while (t < gyroTime)
        {
            t += Time.unscaledDeltaTime;
            gyro.rotation = Quaternion.Slerp(endRotation, startRotation, gyroAnimation.Evaluate(t / gyroTime));

            yield return null;
        }

        gyro.rotation = startRotation;

        gyroAnimationCoroutine = null;
    }

    private void OnDestroy()
    {
        tapAction.action.performed -= OnTap;
        tapAction.action.Disable();
    }
}
