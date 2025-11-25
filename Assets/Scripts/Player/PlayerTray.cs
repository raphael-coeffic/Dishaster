using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TrayState
{
    Empty,
    Food,
    Drinks,
    Trash
}

public enum FoodType
{
    Salad,
    Fish,
    Meat,
    Dessert
}

public enum DrinkType
{
    Water,
    Wine,
    Coktail
}

public class PlayerTray : MonoBehaviour
{
    [SerializeField] private int maxFood = 2;
    [SerializeField] private int maxDrinks = 2;
    public int maxTrash = 6;

    public List<FoodType> carriedFood = new List<FoodType>();
    public List<DrinkType> carriedDrinks = new List<DrinkType>();
    public int currentTrash = 0;

    public TrayState trayState = TrayState.Empty;
    public bool isPretending = false;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (IsEmpty() && Keyboard.current.sKey.isPressed)
            isPretending = true;
        else
            isPretending = false;
    }

    public bool HasServiceItems()
    {
        return carriedFood.Count > 0 || carriedDrinks.Count > 0;
    }

    public bool IsEmpty()
    {
        return carriedFood.Count == 0 &&
               carriedDrinks.Count == 0 &&
               currentTrash == 0;
    }

    public bool IsPretending()
    {
        return isPretending && IsEmpty();
    }

    public void ClearTray()
    {
        carriedFood.Clear();
        carriedDrinks.Clear();
        currentTrash = 0;
        UpdateTrayState();
    }

    public bool CanTakeFood()
    {
        return carriedFood.Count < maxFood && currentTrash == 0;
    }

    public bool CanTakeDrinks()
    {
        return carriedDrinks.Count < maxDrinks && currentTrash == 0;
    }

    public bool CanTakeTrash()
    {
        return currentTrash < maxTrash && !HasServiceItems();
    }

    public void TakeFood(FoodType type)
    {
        if (!CanTakeFood())
            return;

        carriedFood.Add(type);
        UpdateTrayState();
    }

    public void TakeDrinks(DrinkType type)
    {
        if (!CanTakeDrinks())
            return;

        carriedDrinks.Add(type);
        UpdateTrayState();
    }

    public void TakeTrash(int amount)
    {
        if (!CanTakeTrash())
            return;

        int space = maxTrash - currentTrash;
        int toAdd = Mathf.Clamp(amount, 0, space);

        if (toAdd <= 0)
            return;

        currentTrash += toAdd;
        UpdateTrayState();
    }

    private void UpdateTrayState()
    {
        if (IsEmpty())
            trayState = TrayState.Empty;
        else if (currentTrash > 0 && !HasServiceItems())
            trayState = TrayState.Trash;
        else if (carriedFood.Count > 0 && carriedDrinks.Count == 0)
            trayState = TrayState.Food;
        else if (carriedDrinks.Count > 0 && carriedFood.Count == 0)
            trayState = TrayState.Drinks;
        else
            trayState = TrayState.Food;
    }

    public void RefreshState()
    {
        UpdateTrayState();
    }
}
