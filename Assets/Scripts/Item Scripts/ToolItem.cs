using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Tool Item")]
public class ToolItem : InventoryItemData
{
    public float durability;
    public GameObject handPrefab; //the model that goes in the players hand
    public ToolBehavior behavior;

}
