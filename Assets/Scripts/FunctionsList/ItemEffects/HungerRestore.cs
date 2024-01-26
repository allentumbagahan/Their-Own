using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Item_Effect
{
    public static void Restore_Hunger_10Percent(NpcWrapper npcWrapper)
    {
        NpcScript npcScript = npcWrapper.npc;
        npcScript.Hunger_Deducted -= (int)(npcScript.GetInit_Hunger * 0.10f);
    } 
    public static void Restore_Hunger_20Percent(NpcWrapper npcWrapper)
    {
        NpcScript npcScript = npcWrapper.npc;
        npcScript.Hunger_Deducted -= (int)(npcScript.GetInit_Hunger * 0.20f);
    } 
}
