using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class TalentsList
{           
    public static void GatherBerries(NpcWrapper npcWrapper)
    {
        NpcScript npcScript = npcWrapper.npc;
        if(npcScript.Decision == 3)
        {
            npcScript.Decision = 0; // but not idle
            Debug.Log("Foraging");
            ObjectsListData objsList = GameObject.Find("ObjectsList").GetComponent<ObjectsListData>();
            if(objsList.ObjectsList.Count > 1)
            {
                GameObject prevTarget = npcScript.TargetObject;
                npcScript.TargetObject = objsList.GetNearestObject(npcScript.gameObject.transform.position, Enums.EnvironmenttObjectCategory.Bush_FoodSource, true);
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
                if(npcScript.TargetObject != null)
                {
                    if(npcScript.TargetObject.GetComponent<EnvironmentObjectData>().EnvironmenttObjectStatus != Enums.EnvironmenttObjectStatus.Alive)
                    {
                        npcScript.TargetObject = null;
                    }
                }
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
