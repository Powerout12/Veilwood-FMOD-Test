using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Behavior", menuName = "Tool Behavior/Shovel")]
public class ShovelBehavior : ToolBehavior
{
    public LayerMask mask;
    public override void PrimaryUse(Transform player, ToolType tool)
    {
        //swing
        HandItemManager.Instance.PlayPrimaryAnimation();
        //HandItemManager.Instance.toolSource.PlayOneShot(swing);
        Vector3 fwd = player.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(player.position, fwd, out hit, 4, mask))
        {
            //DONT MAKE THIS INSTANT, ALIGN IT WITH ANIMATION
            var structure = hit.collider.GetComponent<StructureBehaviorScript>();
            if (structure != null)
            {
                structure.health -= 2;
            }

            var creature = hit.collider.GetComponent<CreatureBehaviorScript>();
            if (creature != null)
            {
                creature.TakeDamage(25);
            }
        }
    }

    public override void SecondaryUse(Transform player, ToolType tool)
    {
        //dig
        Vector3 fwd = player.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(player.position, fwd, out hit, 4, 1 << 6))
        {
            var structure = hit.collider.GetComponent<StructureBehaviorScript>();
            if (structure != null)
            {
                //play water anim
                bool playAnim = false;
                structure.ToolInteraction(tool, out playAnim);
                if (playAnim)
                {
                    HandItemManager.Instance.PlaySecondaryAnimation();
                    //HandItemManager.Instance.toolSource.PlayOneShot(dig);
                }
            }
        }
    }


}
