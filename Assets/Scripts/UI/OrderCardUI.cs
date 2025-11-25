using TMPro;
using UnityEngine;

public class OrderCardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI[] guestTexts;

    private TableOrder linkedTable;

    public void Init(TableOrder table)
    {
        linkedTable = table;
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        if (linkedTable == null)
            return;

        titleText.text = "TABLE " + linkedTable.name;

        for (int i = 0; i < linkedTable.guests.Length; i++)
        {
            var g = linkedTable.guests[i];

            if (!g.hasOrder)
            {
                guestTexts[i].text = $"G{i + 1}: (no order)";
            }
            else
            {
                string food = g.remainingFood.Count == 0
                    ? "Food: none"
                    : "Food: " + string.Join(", ", g.remainingFood);

                string drinks = g.remainingDrinks.Count == 0
                    ? "Drinks: none"
                    : "Drinks: " + string.Join(", ", g.remainingDrinks);

                guestTexts[i].text = $"G{i + 1}: {food} | {drinks}";
            }
        }
    }

    public void RemoveCard()
    {
        Destroy(gameObject);
    }
}
