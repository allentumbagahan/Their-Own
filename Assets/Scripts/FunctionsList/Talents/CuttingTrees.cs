using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class TalentsList
{           
    public static void CutTrees(NpcWrapper npcWrapper)
    {
        NpcScript npcScript = npcWrapper.npc;
        if(npcScript.Decision == 1)
        {
            npcScript.Decision = 0; // but not idle
            Debug.Log("Cut Trees");
            ObjectsListData objsList = GameObject.Find("ObjectsList").GetComponent<ObjectsListData>();
            if(objsList.ObjectsList.Count > 2)
            {
                GameObject prevTarget = npcScript.TargetObject;
                npcScript.TargetObject = objsList.GetNearestObject(npcScript.gameObject.transform.position, Enums.EnvironmenttObjectCategory.Trees, true);
                if(npcScript.TargetObject != null) 
                {
                    npcScript.TargetObject.GetComponent<EnvironmentObjectData>().SetAsTarget(npcScript.gameObject);
                    if(npcScript.TargetObject != prevTarget)
                    {
                        Vector3 targetPos = new Vector3(npcScript.TargetObject.transform.position.x, npcScript.TargetObject.transform.position.y - 0.04f, npcScript.TargetObject.transform.position.z);
                        npcScript.GetComponent<MovementController2D>().GetMoveCommand(targetPos);
                        npcScript.Status = Enums.StatusType.Moving;
                    }
                }
            }
            
            npcScript.Init_UnivFunc_ConditionToRun = ()=> 
            {
                npcScript.UnivFunc_ConditionToRun = npcScript.Status == Enums.StatusType.ReachedTarget || npcScript.Status == Enums.StatusType.Attacking;
                Debug.Log((npcScript.Status == Enums.StatusType.ReachedTarget) + " " + (npcScript.Status == Enums.StatusType.Attacking) + " run invoke " + npcScript.UnivFunc_ConditionToRun);
            };
            npcScript.Init_UnivFunc_ConditionToStop = ()=> 
            {
                npcScript.UnivFunc_ConditionToStop = npcScript.Status == Enums.StatusType.Idle;
                Debug.Log("Stop invoke");
            };
            npcScript.actionFunction = npcScript.Attack;
            npcScript.UniversalFuncInvokeRepeatedStart(1, 1);
        }
    }

}
