using System.Text;
using UnityEngine;
using TMPro;

public class TrayHUD : MonoBehaviour
{
    [SerializeField] private PlayerTray playerTray;
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI drinksText;
    [SerializeField] private TextMeshProUGUI trashText;

    private StringBuilder sb = new StringBuilder();

    private void Update()
    {
        if (playerTray == null)
            return;

        if (foodText != null)
        {
            sb.Clear();
            if (playerTray.carriedFood.Count == 0)
                sb.Append("Food: (none)");
            else
            {
                sb.Append("Food: ");
                for (int i = 0; i < playerTray.carriedFood.Count; i++)
                {
                    sb.Append(playerTray.carriedFood[i]);
                    if (i < playerTray.carriedFood.Count - 1)
                        sb.Append(", ");
                }
            }
            foodText.text = sb.ToString();
        }

        if (drinksText != null)
        {
            sb.Clear();
            if (playerTray.carriedDrinks.Count == 0)
                sb.Append("Drinks: (none)");
            else
            {
                sb.Append("Drinks: ");
                for (int i = 0; i < playerTray.carriedDrinks.Count; i++)
                {
                    sb.Append(playerTray.carriedDrinks[i]);
                    if (i < playerTray.carriedDrinks.Count - 1)
                        sb.Append(", ");
                }
            }
            drinksText.text = sb.ToString();
        }

        if (trashText != null)
        {
            sb.Clear();
            if (playerTray.currentTrash == 0)
                sb.Append("Trash: (none)");
            else
                sb.Append($"Trash: {playerTray.currentTrash}/{playerTray.maxTrash}");

            trashText.text = sb.ToString();
        }
    }
}
