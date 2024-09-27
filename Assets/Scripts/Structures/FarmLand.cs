using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : StructureBehaviorScript
{
    public CropData crop; //The current crop planted here
    public SpriteRenderer cropRenderer;
    public Transform itemDropTransform;

    public float waterLevel; //How much has this crop been watered
    public int growthStage = -1; //-1 means there is no crop
    public int hoursSpent = 0; //how long has the plant been in this growth stage for?

    public bool harvestable = false; //true if growth stage matches crop data growth stages
    public bool rotted = false;

    private bool ignoreNextGrowthMoment = false; //tick this if crop was just planted
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        SpriteChange();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void ItemInteraction(InventoryItemData item)
    {
        CropItem newCrop = item as CropItem;
        if(newCrop && newCrop.plantable)
        {
            crop = newCrop.cropData;
            growthStage = 1;
            SpriteChange();
            HotbarDisplay.currentSlot.AssignedInventorySlot.RemoveFromStack(1);
        }
    }

    public override void StructureInteraction()
    {
        if(harvestable)
        {
            harvestable = false;
            if(rotted == false)
            {
                for(int i = 0; i < crop.cropYieldAmount; i++)
                {
                    Instantiate(crop.cropYield, itemDropTransform.position, Quaternion.identity);
                }
                for(int i = 0; i < crop.seedYieldAmount; i++)
                {
                    Instantiate(crop.cropSeed, itemDropTransform.position, Quaternion.identity);
                }
            }

            crop = null;
            SpriteChange();
        }
    }

    public override void HourPassed()
    {
        if(ignoreNextGrowthMoment)
        {
            ignoreNextGrowthMoment = false;
            return;
        }
        if(!crop)
        {
            Destroy(this.gameObject);
            return;
        }
        hoursSpent++;
        if(hoursSpent >= crop.hoursPerStage && growthStage < crop.growthStages)
        {
            if(growthStage == crop.growthStages - 1)
            {
                
                if(hoursSpent < crop.hoursPerStage * 3) return;
                //plant rots
                growthStage++;
                rotted = true;
                harvestable = true;
                SpriteChange();
            }
            hoursSpent = 0;
            growthStage++;
            if(growthStage == crop.growthStages - 1) harvestable = true;
            SpriteChange();
        }
    }

    void SpriteChange()
    {
        if(crop) cropRenderer.sprite = crop.cropSprites[growthStage - 1];
        else cropRenderer.sprite = null;
    }
}
