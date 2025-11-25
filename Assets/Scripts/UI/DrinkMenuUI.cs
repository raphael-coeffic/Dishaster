using UnityEngine;
using UnityEngine.UI;

public class DrinkMenuUI : MonoBehaviour
{
    public static DrinkMenuUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Button waterButton;
    [SerializeField] private Button wineButton;
    [SerializeField] private Button cocktailButton;

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

        if (waterButton != null)
            waterButton.onClick.AddListener(() => OnDrinkClicked(DrinkType.Water));

        if (wineButton != null)
            wineButton.onClick.AddListener(() => OnDrinkClicked(DrinkType.Wine));

        if (cocktailButton != null)
            cocktailButton.onClick.AddListener(() => OnDrinkClicked(DrinkType.Coktail));
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

    private void OnDrinkClicked(DrinkType type)
    {
        if (currentTray == null)
            return;

        if (!currentTray.CanTakeDrinks())
            return;

        currentTray.TakeDrinks(type);

        if (!currentTray.CanTakeDrinks())
            Close();
    }
}
