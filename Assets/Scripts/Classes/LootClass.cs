using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot 
{
    public Item item;
    [Tooltip("100 = 100%")]
    public int chance;
    public int MinDrop;
    public int MaxDrop;
    public Loot(Item item, int chance, int min, int max)
    {
        this.item = item;
        this.chance = chance;
        this.MinDrop = min;
        this.MaxDrop = max;
    }

}
