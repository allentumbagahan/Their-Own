using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berries : Item
{
    public Berries()
    {
        itemName = "Berries";
        category = Enums.CategoryType.Food;
        itemEffects += Item_Effect.Restore_Hunger_10Percent;
    }
}
