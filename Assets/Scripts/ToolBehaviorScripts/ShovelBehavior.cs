using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Behavior", menuName = "Tool Behavior/Shovel")]
public class ShovelBehavior : ToolBehavior
{
    public override void PrimaryUse(Transform player, ToolType tool)
    {
        //swing
    }

    public override void SecondaryUse(Transform player, ToolType tool)
    {
        //nothing
    }


}
