using UnityEngine;
using UnityEngine.UI;

public class FoodMenuUI : MonoBehaviour
{
    public static FoodMenuUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Button saladButton;
    [SerializeField] private Button fishButton;
    [SerializeField] private Button meatButton;
    [SerializeField] private Button dessertButton;

    private PlayerTray currentTray;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (panel != null)
            panel.SetActive(false);

        if (saladButton != null)
            saladButton.onClick.AddListener(() => OnFoodClicked(FoodType.Salad));

        if (fishButton != null)
            fishButton.onClick.AddListener(() => OnFoodClicked(FoodType.Fish));

        if (meatButton != null)
            meatButton.onClick.AddListener(() => OnFoodClicked(FoodType.Meat));

        if (dessertButton != null)
            dessertButton.onClick.AddListener(() => OnFoodClicked(FoodType.Dessert));
    }

    public void Open(PlayerTray tray)
    {
        currentTray = tray;

        if (panel != null)
            panel.SetActive(true);
    }

    public void Close()
    {
        currentTray = null;

        if (panel != null)
            panel.SetActive(false);
    }

    private void OnFoodClicked(FoodType type)
    {
        if (currentTray == null)
            return;

        if (!currentTray.CanTakeFood())
            return;

        currentTray.TakeFood(type);

        if (!currentTray.CanTakeFood())
            Close();
    }
}
