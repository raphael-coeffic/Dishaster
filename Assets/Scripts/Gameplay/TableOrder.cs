using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public enum TableState
{
    Idle,
    Calling,
    WaitingServe,
    Served
}

public class TableOrder : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int maxFoodPerGuest = 1;
    [SerializeField] private int maxDrinksPerGuest = 1;

    [Header("Debug")]
    [SerializeField] private bool callOnStart = false;

    [Header("Timing")]
    [SerializeField] private float firstOrderDelaySeconds = 5f;
    [SerializeField] private float clearDelaySeconds = 30f;
    [SerializeField] private float reorderDelaySeconds = 15f;

    [Header("State")]
    public TableState state = TableState.Idle;

    [Header("Guests")]
    public GuestOrder[] guests = new GuestOrder[6];

    [Header("Visual")]
    [SerializeField] private GameObject callIcon;
    [SerializeField] private TextMeshProUGUI orderText;

    private StringBuilder sb = new StringBuilder();
    private Coroutine blinkCoroutine;

    [HideInInspector] public bool isCallingForClear = false;

    private void Reset()
    {
        if (guests == null || guests.Length == 0)
        {
            guests = new GuestOrder[6];
            for (int i = 0; i < guests.Length; i++)
                guests[i] = new GuestOrder { guestName = "Guest " + (i + 1) };
        }
    }

    private void Start()
    {
        if (callIcon != null)
            callIcon.SetActive(false);

        if (callOnStart)
        {
            if (firstOrderDelaySeconds <= 0f)
                StartCallingForOrder();
            else
                StartCoroutine(FirstOrderRoutine());
        }

        UpdateVisuals();
    }

    private System.Collections.IEnumerator FirstOrderRoutine()
    {
        yield return new WaitForSeconds(firstOrderDelaySeconds);

        if (state == TableState.Idle)
            StartCallingForOrder();
    }

    public void StartCalling() => StartCallingForOrder();

    public void StartCallingForOrder()
    {
        state = TableState.Calling;
        isCallingForClear = false;

        if (callIcon != null)
        {
            callIcon.SetActive(true);

            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkCallIcon());
        }

        UpdateVisuals();
    }

    public void StartCallingForClear()
    {
        state = TableState.Calling;
        isCallingForClear = true;

        if (callIcon != null)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            callIcon.SetActive(true);
        }

        UpdateVisuals();
    }

    private System.Collections.IEnumerator BlinkCallIcon()
    {
        while (state == TableState.Calling && !isCallingForClear && callIcon != null)
        {
            callIcon.SetActive(!callIcon.activeSelf);
            yield return new WaitForSeconds(0.4f);
        }

        if (callIcon != null)
            callIcon.SetActive(true);
    }

    public void GenerateOrdersForAllGuests()
    {
        foreach (var guest in guests)
            OrderUtils.GiveFixedOrder(guest);

        state = TableState.WaitingServe;
        isCallingForClear = false;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        if (callIcon != null)
            callIcon.SetActive(false);

        if (OrderUIManager.Instance != null)
        {
            OrderUIManager.Instance.CreateCard(this);
            OrderUIManager.Instance.UpdateCard(this);
        }

        UpdateVisuals();
    }

    public bool ServeFromTray(PlayerTray tray)
    {
        if (state != TableState.WaitingServe)
            return false;

        bool servedSomething = false;

        foreach (var guest in guests)
        {
            if (!guest.hasOrder || guest.isServed)
                continue;

            for (int i = guest.remainingFood.Count - 1; i >= 0; i--)
            {
                FoodType needed = guest.remainingFood[i];
                int idx = tray.carriedFood.FindIndex(x => x == needed);
                if (idx >= 0)
                {
                    tray.carriedFood.RemoveAt(idx);
                    guest.remainingFood.RemoveAt(i);
                    servedSomething = true;
                }
            }

            for (int i = guest.remainingDrinks.Count - 1; i >= 0; i--)
            {
                DrinkType neededDrink = guest.remainingDrinks[i];
                int idx = tray.carriedDrinks.FindIndex(x => x == neededDrink);
                if (idx >= 0)
                {
                    tray.carriedDrinks.RemoveAt(idx);
                    guest.remainingDrinks.RemoveAt(i);
                    servedSomething = true;
                }
            }

            if (guest.remainingFood.Count == 0 &&
                guest.remainingDrinks.Count == 0 &&
                guest.hasOrder)
            {
                guest.isServed = true;
            }
        }

        if (servedSomething)
        {
            tray.RefreshState();

            bool allServed = true;
            foreach (var guest in guests)
            {
                if (guest.hasOrder && !guest.isServed)
                {
                    allServed = false;
                    break;
                }
            }

            if (allServed)
                MarkAsServed();
            else
                NotifyOrdersChanged();
        }

        return servedSomething;
    }

    public void NotifyOrdersChanged()
    {
        UpdateVisuals();

        if (OrderUIManager.Instance != null &&
            (state == TableState.WaitingServe || state == TableState.Served))
        {
            OrderUIManager.Instance.UpdateCard(this);
        }

        if (OrderUIManager.Instance != null && state == TableState.Idle)
        {
            OrderUIManager.Instance.RemoveCard(this);
        }
    }

    public void UpdateVisuals()
    {
        if (orderText != null)
        {
            sb.Clear();

            if (state == TableState.Idle)
            {
                sb.Append("Idle");
            }
            else if (state == TableState.Calling)
            {
                sb.Append(isCallingForClear ? "Calling (clear)..." : "Calling (order)...");
            }
            else if (state == TableState.WaitingServe)
            {
                for (int i = 0; i < guests.Length; i++)
                {
                    var g = guests[i];
                    sb.Append($"G{i + 1}: ");

                    if (!g.hasOrder)
                    {
                        sb.Append("(no order)");
                    }
                    else
                    {
                        if (g.remainingFood.Count == 0)
                        {
                            sb.Append("Food: (none)");
                        }
                        else
                        {
                            sb.Append("Food: ");
                            for (int f = 0; f < g.remainingFood.Count; f++)
                            {
                                sb.Append(g.remainingFood[f]);
                                if (f < g.remainingFood.Count - 1)
                                    sb.Append(", ");
                            }
                        }

                        sb.Append(" | Drinks: ");
                        if (g.remainingDrinks.Count == 0)
                        {
                            sb.Append("(none)");
                        }
                        else
                        {
                            for (int d = 0; d < g.remainingDrinks.Count; d++)
                            {
                                sb.Append(g.remainingDrinks[d]);
                                if (d < g.remainingDrinks.Count - 1)
                                    sb.Append(", ");
                            }
                        }
                    }

                    sb.AppendLine();
                }
            }
            else if (state == TableState.Served)
            {
                sb.Append("Served ✔");
            }

            orderText.text = sb.ToString();
        }

        if (OrderUIManager.Instance != null &&
            (state == TableState.WaitingServe || state == TableState.Served))
        {
            OrderUIManager.Instance.UpdateCard(this);
        }

        if (OrderUIManager.Instance != null && state == TableState.Idle)
        {
            OrderUIManager.Instance.RemoveCard(this);
        }
    }

    public void MarkAsServed()
    {
        state = TableState.Served;
        isCallingForClear = false;

        if (GameManager.Instance != null)
            GameManager.Instance.AddMoneyForTable();

        if (OrderUIManager.Instance != null)
            OrderUIManager.Instance.RemoveCard(this);

        UpdateVisuals();

        StartCoroutine(ClearAndRecallRoutine());
    }

    private System.Collections.IEnumerator ClearAndRecallRoutine()
    {
        yield return new WaitForSeconds(clearDelaySeconds);

        if (state == TableState.Served)
            StartCallingForClear();
    }

    public void MarkCleared()
    {
        state = TableState.Idle;
        isCallingForClear = false;

        foreach (var g in guests)
        {
            g.hasOrder = false;
            g.isServed = false;
            g.remainingFood.Clear();
            g.remainingDrinks.Clear();
        }

        if (callIcon != null)
            callIcon.SetActive(false);

        UpdateVisuals();

        if (reorderDelaySeconds <= 0f)
        {
            StartCallingForOrder();
        }
        else
        {
            StartCoroutine(ReorderRoutine());
        }
    }

    private System.Collections.IEnumerator ReorderRoutine()
    {
        yield return new WaitForSeconds(reorderDelaySeconds);

        if (state == TableState.Idle)
            StartCallingForOrder();
    }
}
