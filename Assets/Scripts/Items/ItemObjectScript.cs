using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{

    protected delegate void ItemEffect(NpcWrapper npcWrapper);
    [SerializeField] protected string itemName;
    [SerializeField] protected Enums.CategoryType category;
    protected ItemEffect itemEffects;

    public global::System.String ItemName { get => itemName;  }
    protected ItemEffect ItemEffects { get => itemEffects; set => itemEffects += value;}
    public Enums.CategoryType Category {get => category;}

    public void Use(NpcWrapper npcWrapper)
    {
        itemEffects.Invoke(npcWrapper);
    }
}
