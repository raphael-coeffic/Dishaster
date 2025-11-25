using UnityEngine;
using System.Collections.Generic;

public class OrderUIManager : MonoBehaviour
{
    public static OrderUIManager Instance { get; private set; }

    [SerializeField] private Transform orderPanel;
    [SerializeField] private GameObject orderCardPrefab;

    private Dictionary<TableOrder, OrderCardUI> cards = new Dictionary<TableOrder, OrderCardUI>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreateCard(TableOrder table)
    {
        if (cards.ContainsKey(table))
            return;

        GameObject go = Instantiate(orderCardPrefab, orderPanel);
        OrderCardUI card = go.GetComponent<OrderCardUI>();

        card.Init(table);
        cards.Add(table, card);
    }

    public void UpdateCard(TableOrder table)
    {
        if (cards.ContainsKey(table))
            cards[table].RefreshDisplay();
    }

    public void RemoveCard(TableOrder table)
    {
        if (!cards.ContainsKey(table))
            return;

        cards[table].RemoveCard();
        cards.Remove(table);
    }
}
