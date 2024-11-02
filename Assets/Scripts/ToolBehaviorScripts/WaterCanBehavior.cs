using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Behavior", menuName = "Tool Behavior/WaterCan")]
public class WaterCanBehavior : ToolBehavior
{
    public AudioClip refill, pour;
    public override void PrimaryUse(Transform _player, ToolType _tool)
    {
        if (usingPrimary || usingSecondary || PlayerInteraction.Instance.toolCooldown) return;
        if (!player) player = _player;
        tool = _tool;
        //water
        Vector3 fwd = player.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(player.position, fwd, out hit, 4, mask))
        {
            var structure = hit.collider.GetComponent<StructureBehaviorScript>();
            if (structure != null)
            {
                //play water anim
                bool playAnim = false;
                structure.ToolInteraction(tool, out playAnim);
                if(playAnim)
                {
                    HandItemManager.Instance.PlayPrimaryAnimation();
                    HandItemManager.Instance.toolSource.PlayOneShot(pour);
                    PlayerInteraction.Instance.StartCoroutine(PlayerInteraction.Instance.ToolUse(this, 0.8f, 1.3f));
                    PlayerMovement.restrictMovementTokens++;
                } 
            }
        }
    }

    public override void SecondaryUse(Transform _player, ToolType _tool)
    {
        //nothing
    }

    public override void ItemUsed() 
    { 
        PlayerMovement.restrictMovementTokens--;
    }


}
