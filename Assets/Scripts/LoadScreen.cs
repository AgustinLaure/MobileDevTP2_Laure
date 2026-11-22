using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadText;
    [SerializeField] private float loadAnimationTime;
    [SerializeField] private float minLoadingTime;
    private GameManager gameManager;

    private Coroutine loadAnimation = null;

    private const string loadingText = "Loading";

    private WaitForSecondsRealtime loadAnimationTimer;

    private void Awake()
    {
        gameManager = ServiceLocator.Instance.GetService<GameManager>();

        loadAnimationTimer = new WaitForSecondsRealtime(loadAnimationTime);
    }

    private void Start()
    {
        StartCoroutine(LoadCoroutine());
        Debug.Log("StartedLoadScreen");
    }

    private IEnumerator LoadCoroutine()
    {
        yield return null;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(gameManager.TargetScene);
        loadOperation.allowSceneActivation = false;

        float timer = minLoadingTime;

        while (loadOperation.progress < 0.9f || timer > 0f)
        {
            timer -= Time.unscaledDeltaTime;

            if (loadAnimation == null)
            {
                loadAnimation = StartCoroutine(LoadAnimation());
            }

            yield return null;
        }

        yield return gameManager.FadeOut();

        Debug.Log("EndLoadScreen");

        loadOperation.allowSceneActivation = true;
    }

    private IEnumerator LoadAnimation()
    {
        for (int i = 0; i < 3; i++)
        {
            string currentText = loadingText;

            for (int j = 0; j < i; j++)
            {
                currentText += ".";
            }

            loadText.text = currentText;

            yield return loadAnimationTimer;
        }

        loadAnimation = null;
    }
}
