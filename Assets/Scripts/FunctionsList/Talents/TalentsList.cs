using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TalentsList : MonoBehaviour
{

    /*public void sampleFunction(NpcWrapper npcWrappe)
    {
        
    }*/
    public delegate void talentFunction(NpcWrapper npcWrapper);
    public Dictionary<string, talentFunction> List_Talents;
    void OnStart()
    {
        List_Talents = new Dictionary<string, talentFunction>();
        List_Talents["Cutting_Trees"] = CutTrees;
    }

}
