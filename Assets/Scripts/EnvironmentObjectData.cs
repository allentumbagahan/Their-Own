using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectData : MonoBehaviour
{

    [SerializeField] private Enums.EnvironmenttObjectCategory category;
    [Header("RandomPosition")]
    [SerializeField] private float randomMaxX;
    [SerializeField] private float randomMaxY;
    [Header("Fixed Position")]
    [SerializeField] private bool fixedPosition;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    [Space][Header("Attributes")]
    [SerializeField] private int Init_HitPoints;
    [Space][Header("Settings")]
    [SerializeField] private bool IsDestroyWhenDie;
    [Header("IsDestroyWhenDie if False")]
    [SerializeField] private Sprite OnDie_Sprite;
    [SerializeField] private Sprite OnRestored_Sprite;
    [SerializeField] private bool IsRestoringHPWhenDie;
    [Tooltip("Percentage To Restore every Second")][SerializeField] private int Init_RestoreHP_PerSec;
    [SerializeField] private Enums.EnvironmenttObjectCategory OnRestoreHP_Category;
    [SerializeField] private Enums.EnvironmenttObjectCategory OnDie_Category;
    
    
    public global::System.Single RandomMaxX { get => randomMaxX; set => randomMaxX = value; }
    public global::System.Single RandomMaxY { get => randomMaxY; set => randomMaxY = value; }
    public Enums.EnvironmenttObjectCategory Category { get => category; set => category = value; }
    public global::System.Boolean FixedPosition { get => fixedPosition; set => fixedPosition = value; }
    public global::System.Single OffsetX { get => offsetX; set => offsetX = value; }
    public global::System.Single OffsetY { get => offsetY; set => offsetY = value; }
    public Enums.EnvironmenttObjectStatus EnvironmenttObjectStatus { get => environmenttObjectStatus; set => environmenttObjectStatus = value; }

    public void SetAsTarget(GameObject comingObject)
    {
        comingObjects.Add(comingObject);
    }   


    [Header("Calculated Data")]
    public List<GameObject> comingObjects = new List<GameObject>();
    [SerializeField]
    internal float resX;
    [SerializeField]
    internal float resY;
    [SerializeField]
    internal Vector3 cellPos;
    [SerializeField]
    internal int hitPoints;
    [SerializeField] internal int Restoring_StartTime = 0;
    [SerializeField] private Enums.EnvironmenttObjectStatus environmenttObjectStatus = Enums.EnvironmenttObjectStatus.Alive;
    void Start()
    {
        hitPoints = Init_HitPoints;
    }
    public Reward TakeDamage(int damage)
    {
        Debug.Log(this + " Taking Damage");
        Reward reward = new Reward();
        hitPoints -= damage;
        Quaternion tilt = Quaternion.Euler(new Vector3(0,0, UnityEngine.Random.Range(-10, 10)));
        transform.rotation = tilt;
        Invoke("defaultRotation", 1.0f);
        if(hitPoints < 0) {
            reward.Item_Loots = GetComponent<Loots>().GetLoot();
            DieOrDestroy();
        }
        return reward;
    }
    public void DieOrDestroy()
    {
        if(IsDestroyWhenDie)
        {
            Destroy(gameObject);
        }
        else
        {
            category = OnDie_Category;
            EnvironmenttObjectStatus = Enums.EnvironmenttObjectStatus.Restoring;
            GetComponent<SpriteRenderer>().sprite = OnDie_Sprite;
            defaultRotation();
        }
    }
    void defaultRotation()
    {
        Quaternion defaultRot = Quaternion.Euler(new Vector3(0,0, UnityEngine.Random.Range(-10, 10)));
        transform.rotation = defaultRot;
    }
    void FixedUpdate()
    {
        try
        {
            foreach (GameObject item in comingObjects)
            {
                if(item.GetComponent<NpcScript>().TargetObject != gameObject)
                {
                    comingObjects.Remove(item);
                }
            }
        }
        catch (System.Exception)
        {
 
        }
        RestoringHp();

    }
    void RestoringHp()
    {
        TimeCounter Time = GameObject.Find("Time").GetComponent<TimeCounter>();
        if(EnvironmenttObjectStatus == Enums.EnvironmenttObjectStatus.Restoring)
        {
            if(Restoring_StartTime == 0)
            {
                Restoring_StartTime = Time.GetTime;
            }
            if(Init_HitPoints == hitPoints)
            {
                Restoring_StartTime = 0;
                category = OnRestoreHP_Category;
                EnvironmenttObjectStatus = Enums.EnvironmenttObjectStatus.Alive;
                GetComponent<SpriteRenderer>().sprite = OnRestored_Sprite;
                defaultRotation();
            }
            else 
            {
                float RestoredHP = ((Time.GetTime - Restoring_StartTime) * Init_RestoreHP_PerSec)/100.0f;
                if(RestoredHP > 1) RestoredHP = 1;
                hitPoints = (int)(Init_HitPoints*RestoredHP);
                Debug.Log(this + " Restoring " + RestoredHP );
            }
            
        }
    }

}
