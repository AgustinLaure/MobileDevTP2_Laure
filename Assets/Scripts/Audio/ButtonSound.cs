using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private Button button;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = ServiceLocator.Instance.GetService<GameManager>();

        button = GetComponent<Button>();
        button.onClick.AddListener(HandleButtonPress);
    }

    private void HandleButtonPress()
    {
        gameManager.ButtonSound.Play();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(HandleButtonPress);
    }
}
