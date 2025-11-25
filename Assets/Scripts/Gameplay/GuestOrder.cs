using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuestOrder
{
    public string guestName = "Guest";

    public List<FoodType> remainingFood = new List<FoodType>();
    public List<DrinkType> remainingDrinks = new List<DrinkType>();

    public bool hasOrder = false;
    public bool isServed = false;
}
