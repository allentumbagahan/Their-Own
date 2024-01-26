using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[ExecuteInEditMode]
public class TalenBook : MonoBehaviour
{
    [SerializeField]
    public TalentsList talentsList = new TalentsList();
    public TalentUnityEvent talents;
    [Header("Add Talent To Learn")]
    public bool CuttingTrees;
    void UseBook(){
        //MyScriptableObject newObject = ScriptableObject.CreateInstance<MyScriptableObject>(); //create scriptableobject
    }
}
