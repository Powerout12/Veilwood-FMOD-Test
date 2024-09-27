using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBehaviorScript : MonoBehaviour
{
    //This is the base class that ALL structures should derive from
    StructureManager structManager;


    public StructureObject structData;

    public float health = 5;
    public float maxHealth = 5;


    public void Awake()
    {
        structManager = FindObjectOfType<StructureManager>();
        structManager.allStructs.Add(this);
    }


    public void Update()
    {
        if(health <= 0) Destroy(this.gameObject);
    }

    public virtual void StructureInteraction(){}
    public virtual void ItemInteraction(InventoryItemData item){}
    public virtual void HourPassed(){}

    void OnDestroy()
    {
        if(!gameObject.scene.isLoaded) return;
        print("Destroyed");
        structManager.ClearTile(transform.position);
        structManager.allStructs.Remove(this);
    }
}
