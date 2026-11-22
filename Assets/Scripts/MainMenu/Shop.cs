using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private int hp;
    [SerializeField] private int maxPurchases;
    [SerializeField] private int priceMultiplier;
    [SerializeField] private float buyButtonDisabledAlpha;
    [SerializeField] private float currentHealthDisabledAlpha;

    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private CanvasGroup buyButtonCG;

    [SerializeField] private CanvasGroup[] currentHealthCG;

    private GameManager gameManager;
    private EventBus eventBus;

    private const string maxText = "Max";

    public int GetPrice { get { return price; } }
    public int GetHp { get { return hp; } }

    private void Awake()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        gameManager = serviceLocator.GetService<GameManager>();
        eventBus = serviceLocator.GetService<EventBus>();
    }

    private void Start()
    {
        UpdateCurrentHealth();

        if (gameManager.GetPlayerHealthPoints.maxHp >= maxPurchases)
        {
            DisableButton();
            priceText.text = maxText;
        }
        else
        {
            priceText.text = price.ToString();
            if (gameManager.GetCurrentMoney < price)
            {
                DisableButton();
            }
        }

        buyButton.onClick.AddListener(HandleBuy);
    }

    private void UpdateCurrentHealth()
    {
        for (int i = 0; i < currentHealthCG.Length; i++)
        {
            currentHealthCG[i].alpha = i < gameManager.GetPlayerHealthPoints.maxHp ? 1f : currentHealthDisabledAlpha;
        }
    }

    private void HandleBuy()
    {
        gameManager.BuyHealth(hp, price);
        price *= priceMultiplier;
        priceText.text = price.ToString();
        eventBus.Raise<OnMainMenuUpdateMoney>();

        if (gameManager.GetPlayerHealthPoints.maxHp >= maxPurchases)
        {
            DisableButton();
            priceText.text = maxText;
        }
        else
        {
            if (gameManager.GetCurrentMoney < price)
            {
                DisableButton();
            }
        }

        UpdateCurrentHealth();
    }

    private void DisableButton()
    {
        UiUtils.SetCanvasActive(buyButtonCG, false);
        buyButtonCG.alpha = buyButtonDisabledAlpha;
    }

    private void OnDestroy()
    {
        buyButton.onClick.RemoveListener(HandleBuy);
    }
}
