using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Item_Effect : MonoBehaviour
{
    public delegate void itemEffectFuntion(NpcWrapper npcWrapper);
    public Dictionary<string, itemEffectFuntion> List_ItemEffects;
    void OnStart()
    {
        List_ItemEffects = new Dictionary<string, itemEffectFuntion>();
        List_ItemEffects["Restore 10% Hunger"] = Restore_Hunger_10Percent;
        List_ItemEffects["Restore 20% Hunger"] = Restore_Hunger_20Percent;
    }
    
}
