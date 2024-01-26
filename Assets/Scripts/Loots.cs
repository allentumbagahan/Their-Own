using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Loots : MonoBehaviour
{
    public List<Loot> lootsList;
    public List<Item> GetLoot()
    {
        List<Item> ItemReward = new List<Item>();
        List<Loot> lootRewarded = lootsList.Where(loot => loot.chance >= UnityEngine.Random.Range(1, 101)).ToList();
        if(lootRewarded.Count > 0)
        {
            foreach (Loot loot in lootRewarded)
            {
                int LootQuantity = UnityEngine.Random.Range(loot.MinDrop, loot.MaxDrop + 1);
                for (int i = 1; i <= LootQuantity; i++)
                {
                    ItemReward.Add(loot.item);
                }
            }
        }
        return ItemReward;
    }
}
