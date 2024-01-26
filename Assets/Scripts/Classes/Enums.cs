using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum StatusType {
        Idle,
        Moving,
        Attacking,
        ReachedTarget,
        AttackCoolingdown,
        Hungry,
        Eating,
        Tired
    }
    public enum EnvironmenttObjectCategory {
        Trees,
        Minerals,
        Grass,
        Flowers,
        Bush_FoodSource,
        Bush,
        Building_House,
    }

    //for items category type
    public enum CategoryType
    {
        Food,
        Resource
    };

    public enum NpcType
    {
        Ally,
        Monster,
        Animal
    };
    public enum AnimalType
    {
        Chicken
    };

    // for Object Status
    public enum EnvironmenttObjectStatus
    {
        Alive,
        Restoring,
        //Replenishing,
        NeedResource,
        DieOrDestroy
    };
}
