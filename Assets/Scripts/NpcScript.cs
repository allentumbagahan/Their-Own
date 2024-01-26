using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable] public class NpcWrapper { public NpcScript npc; }
[System.Serializable] public class TalentUnityEvent : UnityEvent<NpcWrapper> { }
public class NpcScript : MonoBehaviour
{
    [Header("Npc Attributes")]
    [Space] [Header("Name")]
    [SerializeField] private string npcName;
    [Space] [Header("Age")]
    [SerializeField] private int age;
    [Space] [Header("Cash")]
    [SerializeField] private float cash;
    [Space] [Header("Attack")]
    [SerializeField] private int Init_AttackPoints;
    [SerializeField] private int attackPointsMultiplier; //In Percentage 100 == 100%
    public int AttackPoints { get => Init_AttackPoints + ((attackPointsMultiplier/100) * Init_AttackPoints); }
    
    [Space] [Header("Hunger")]
    [SerializeField] private int Init_Hunger;
    [SerializeField] private int hungerDepletion_PerAttack;
    [SerializeField] private int hungerReduction; //In Percentage 100 == 100%
    [SerializeField] private int hunger_Deducted;
    public int Hunger { get => Init_Hunger - hunger_Deducted;  }
    public int GetInit_Hunger {get => Init_Hunger;}
    public global::System.Int32 HungerReduction { get => hungerReduction; set => hungerReduction = value; }
    public global::System.Int32 HungerDepletion_PerAttack { get => hungerDepletion_PerAttack - (hungerDepletion_PerAttack*(hungerReduction/100)); set => hungerDepletion_PerAttack = value; }
    public global::System.Int32 Hunger_Deducted { get => hunger_Deducted; set => hunger_Deducted = value; }
    [Space] [Header("HP")]
    [SerializeField] private int Init_HitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int hitPoints_Deducted;
    public global::System.Int32 HitPoints { get => hitPoints; set => hitPoints = value; }
    [Space] [Header("Tiredness")]
    [SerializeField] private int Init_Tiredness;
    [SerializeField] private int tiredness_Deducted;
    public int Tiredness { get => Init_Tiredness - tiredness_Deducted; }
    public int Tiredness_Deducted { get => tiredness_Deducted; set => tiredness_Deducted = value; }
    //[SerializeField] private int attackPoints; // Init_AttackPoints + ((attackPointsMultiplier/100) * Init_AttackPoints)
    [Space] [Header("Calculating Data")]
    [SerializeField] private int PrevDecision;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Enums.StatusType status = Enums.StatusType.Idle;
    [SerializeField] private int decision;
    [SerializeField] private TalentUnityEvent decisionsFunction;
    private delegate NpcScript PassiveSkill(NpcScript _npc);
    private delegate NpcScript UltimateSkill(NpcScript _npc);

    private List<PassiveSkill> passiveSkills;
    private UltimateSkill ultimateSkill;
    public delegate void ActionFunction();
    
    public ActionFunction actionFunction;
    public ActionFunction Init_UnivFunc_ConditionToRun;
    public ActionFunction Init_UnivFunc_ConditionToStop;
    int TimeWhenUnivFuncInvoke = 0;
    public bool UnivFunc_ConditionToStop;
    public bool UnivFunc_ConditionToRun;
    public List<Item> Inventory;
    TimeCounter Time;

    ObjectsListData objsList;

    public global::System.String Name { get => name; set => name = value; }
    public global::System.Int32 Age { get => age; set => age = value; }
    public global::System.Single Cash { get => cash; set => cash = value; }
    public GameObject TargetObject { get => targetObject; set => targetObject = value; }
    public Enums.StatusType Status { get => status; set => status = value; }
    public global::System.Int32 Decision { get => decision; set => decision = value; }

    void Start()
    {
        InitializeAttributes();
        objsList = GameObject.Find("ObjectsList").GetComponent<ObjectsListData>();
        TargetObject = null;


    }
    void InitializeAttributes()
    {
        hunger_Deducted = 0;
        Time = GameObject.Find("Time").GetComponent<TimeCounter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Status == Enums.StatusType.Moving)
        {
            if(GetComponent<MovementController2D>().isTargetReached)
            {
                Status = Enums.StatusType.ReachedTarget;
            }
        }
        if(Status == Enums.StatusType.Eating)
        {
            Status = Enums.StatusType.Idle;
        }
        if(Status == Enums.StatusType.Idle)
        {
            Decision = UnityEngine.Random.Range(0, 51); // 0 = none, 1=cutTrees
            NpcWrapper npcWrapper = new NpcWrapper();
            npcWrapper.npc = this; // Replace with your logic to get the NpcScript instance
            decisionsFunction.Invoke(npcWrapper);
        }
        if(Status != Enums.StatusType.Hungry && Hunger <= 0 && Status != Enums.StatusType.Moving &&  Status != Enums.StatusType.ReachedTarget)
        {
            Status = Enums.StatusType.Hungry;
            List<Item> FoodsInInventory = Inventory.Where(obj => obj.GetComponent<Item>().Category == Enums.CategoryType.Food).ToList();
            Debug.Log(FoodsInInventory.Count + "foods");
            if(FoodsInInventory.Count > 0)
            {
                Status = Enums.StatusType.Eating;
                //<SAMPLE HOW TO USE ITEM>  
                NpcWrapper npcWrapper = new NpcWrapper();
                npcWrapper.npc = this; 
                FoodsInInventory[0].Use(npcWrapper);
                Inventory.Remove(FoodsInInventory[0]);
            }
            else
            {
                ObjectsListData objsList = GameObject.Find("ObjectsList").GetComponent<ObjectsListData>();
                TargetObject = objsList.GetNearestObject(gameObject.transform.position, Enums.EnvironmenttObjectCategory.Building_House);
                if(TargetObject != null)
                {
                    GetComponent<MovementController2D>().GetMoveCommand(TargetObject.transform.position);
                    Status = Enums.StatusType.Moving;
                }
            }
            
        }
    }
    void DealDamage()
    {
        try
        {
            if(Hunger > 0 && TargetObject != null)
            {
                Debug.Log("Dealing Damage");
                Reward Object_Reward = new Reward();
                Object_Reward = TargetObject.GetComponent<EnvironmentObjectData>().TakeDamage(AttackPoints); // deal damage and get the reward
                Hunger_Deducted += HungerDepletion_PerAttack;
                Tiredness_Deducted += 1;
                foreach (Item item in Object_Reward.Item_Loots)
                {
                    Inventory.Add(item); // add reward to inventory
                }
                Debug.Log("Hunger decrease : " + HungerDepletion_PerAttack + " Target : " + TargetObject);
            }
        }
        catch (System.Exception err)
        {
            Debug.Log("Dealing Damage Occured an Error" + err);
        }
    }
    public void Attack()
    {
        if(Status == Enums.StatusType.Attacking)
        {
            if(TargetObject == null)
            {
                Status = Enums.StatusType.Idle;
                Decision = 0;
            }
        }
        if(TargetObject != null ){
            if(TargetObject.GetComponent<EnvironmentObjectData>() != null )
            {
                Invoke("DealDamage", 1.0f);
            }
            else if(TargetObject.GetComponent<NpcScript>() != null)
            {
                //target is npc
            
            }
            Status = Enums.StatusType.Attacking;
        }
    }
    void makeDecision(){

    }
    void UniversalFunction()
    {
        bool isConditioToStartNotNull = Init_UnivFunc_ConditionToRun != null && Init_UnivFunc_ConditionToStop != null;
        if(TimeWhenUnivFuncInvoke != Time.GetTime && isConditioToStartNotNull)
        {
            TimeWhenUnivFuncInvoke = Time.GetTime;
            Debug.Log("UniversalFunction");
            ActionFunction UnivFunc_ConditionToRunTemp = Init_UnivFunc_ConditionToRun;
            ActionFunction UnivFunc_ConditionToStopTemp = Init_UnivFunc_ConditionToStop;
            UnivFunc_ConditionToRunTemp.Invoke();
            UnivFunc_ConditionToStopTemp.Invoke();
            if(UnivFunc_ConditionToRun)
            {
                actionFunction.Invoke();
                if(UnivFunc_ConditionToStop)
                {    
                    StopUniversalFunction();
                }
            }
        }
    }
    public void UniversalFuncInvokeRepeatedStart(float delay, float invokeEvery)
    {
        InvokeRepeating("UniversalFunction", delay, invokeEvery);
    }
    void StopUniversalFunction()
    {
        Init_UnivFunc_ConditionToStop = null;
        Init_UnivFunc_ConditionToRun = null;
        CancelInvoke("UniversalFunction");
    }
}
