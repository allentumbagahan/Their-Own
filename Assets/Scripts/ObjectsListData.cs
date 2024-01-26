using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsListData : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsList;

    public List<GameObject> ObjectsList { get => objectsList; set => objectsList = value; }

    public void AddObject(GameObject obj)
    {
        objectsList.Add(obj);
    }
    public void RemoveObject(GameObject obj)
    {
        objectsList.Remove(obj);
    }
    public GameObject GetNearestObject(Vector3 mainObjPosition)
    {
        RemoveNullObject();
        List<GameObject> listTemp = objectsList;
        listTemp.Sort((obj1, obj2) => (Vector3.Distance(obj1.transform.position, mainObjPosition)).CompareTo(Vector3.Distance(obj2.transform.position, mainObjPosition)));
        if(listTemp.Count == 0) return null;
        else return listTemp[0];
    }
    public GameObject GetNearestObject(Vector3 mainObjPosition, Enums.EnvironmenttObjectCategory environmenttObjectCategory)
    {
        RemoveNullObject();
        List<GameObject> listTemp = objectsList.Where(obj => obj.GetComponent<EnvironmentObjectData>().Category == environmenttObjectCategory).ToList();
        listTemp.Sort((obj1, obj2) => (Vector3.Distance(obj1.transform.position, mainObjPosition)).CompareTo(Vector3.Distance(obj2.transform.position, mainObjPosition)));
        if(listTemp.Count == 0) return null;
        else return listTemp[0];
    }
    public GameObject GetNearestObject(Vector3 mainObjPosition, Enums.EnvironmenttObjectCategory environmenttObjectCategory, bool noComing)
    {
        RemoveNullObject();
        List<GameObject> listTemp = objectsList.Where(obj => obj.GetComponent<EnvironmentObjectData>().Category == environmenttObjectCategory).ToList();
        if(noComing) listTemp = listTemp.Where(obj => obj.GetComponent<EnvironmentObjectData>().comingObjects.Count == 0).ToList();
        listTemp.Sort((obj1, obj2) => (Vector3.Distance(obj1.transform.position, mainObjPosition)).CompareTo(Vector3.Distance(obj2.transform.position, mainObjPosition)));
        if(listTemp.Count == 0) return null;
        else return listTemp[0];
    }
    void RemoveNullObject()
    {
        List<GameObject> nullObjectsTemp = objectsList.Where(obj => obj  == null).ToList();
        foreach (GameObject nullObject in nullObjectsTemp)
        {
            RemoveObject(nullObject);
        }
    }
}
