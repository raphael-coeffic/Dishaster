using UnityEngine;

public static class OrderUtils
{
    public static FoodType GetRandomFood()
    {
        int max = System.Enum.GetValues(typeof(FoodType)).Length;
        int index = Random.Range(0, max);
        return (FoodType)index;
    }

    public static DrinkType GetRandomDrink()
    {
        int max = System.Enum.GetValues(typeof(DrinkType)).Length;
        int index = Random.Range(0, max);
        return (DrinkType)index;
    }

    public static void GiveFixedOrder(GuestOrder guest)
    {
        guest.remainingFood.Clear();
        guest.remainingDrinks.Clear();
        guest.isServed = false;

        guest.remainingFood.Add(GetRandomFood());
        guest.remainingDrinks.Add(GetRandomDrink());

        guest.hasOrder = true;
    }
}


