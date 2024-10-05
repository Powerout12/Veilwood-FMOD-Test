using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonMerchantNPC : NPC, ITalkable
{

    public override void Interact(PlayerInteraction interactor, out bool interactSuccessful)
    {
        Talk();
        interactSuccessful = false;
        Debug.Log("NPC Interact Successful");
    }

    public void Talk()
    {
        dialogueController.currentTalker = this;
        dialogueController.DisplayNextParagraph(dialogueText, currentPath);
    }

    public override void InteractWithItem(PlayerInteraction interactor, out bool interactSuccessful, InventoryItemData item)
    {
        if(item.sellValueMultiplier == 0 || item.value == 0)
        {
            //Cannot Buy
            currentPath = 1;
            Talk();
        }
        else
        {
            //Can Buy
            currentPath = 0;
            Talk();
        }
        interactSuccessful = true;
    }
}
