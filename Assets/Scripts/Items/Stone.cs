using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Item
{
    public Stone()
    {
        itemName = "Stone";
        category = Enums.CategoryType.Resource;
    }
}
