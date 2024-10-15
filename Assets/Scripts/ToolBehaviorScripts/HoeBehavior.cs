using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Behavior", menuName = "Tool Behavior/Hoe")]
public class HoeBehavior : ToolBehavior
{
    public override void PrimaryUse(Transform player, ToolType _tool)
    {
        //till ground
    }

    public override void SecondaryUse(Transform player, ToolType _tool)
    {
        //swing?
    }

    public override void ItemUsed() { }


}
