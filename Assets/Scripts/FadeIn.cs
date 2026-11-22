using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = ServiceLocator.Instance.GetService<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float t = 1f;
        Color startColor = gameManager.Fade.color;
        startColor.a = 1f;

        yield return null;

        while (t > 0)
        {
            t -= Mathf.Min(Time.unscaledDeltaTime, 0.05f) / gameManager.GetFadeTime;
            startColor.a = t;
            gameManager.Fade.color = startColor;
            yield return null;
        }

        startColor.a = 0f;
        gameManager.Fade.color = startColor;
    }
}
