using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : StructureBehaviorScript
{
    public CropData crop; //The current crop planted here
    public SpriteRenderer cropRenderer;
    public Transform itemDropTransform;

    public MeshRenderer meshRenderer;
    public Material dry, wet, barren, barrenWet;

    public float waterLevel; //How much has this crop been watered
    public int growthStage = -1; //-1 means there is no crop
    public int hoursSpent = 0; //how long has the plant been in this growth stage for?

    public bool harvestable = false; //true if growth stage matches crop data growth stages
    public bool rotted = false;

    private bool ignoreNextGrowthMoment = false; //tick this if crop was just planted

    PlayerInventoryHolder playerInventoryHolder;

    private NutrientStorage nutrients;
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        SpriteChange();
    }

    void Start()
    {
        if(!crop) ignoreNextGrowthMoment = true;
        playerInventoryHolder = FindObjectOfType<PlayerInventoryHolder>();

        nutrients = StructureManager.Instance.FetchNutrient(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void ItemInteraction(InventoryItemData item)
    {
        if(crop) return;
        CropItem newCrop = item as CropItem;
        if(newCrop && newCrop.plantable)
        {
            crop = newCrop.cropData;
            growthStage = 1;
            SpriteChange();
            HotbarDisplay.currentSlot.AssignedInventorySlot.RemoveFromStack(1);
            playerInventoryHolder.UpdateInventory();

        }
    }

    public override void StructureInteraction()
    {
        if(harvestable)
        {
            harvestable = false;
            if(rotted == false)
            {
                GameObject droppedItem;
                for(int i = 0; i < crop.cropYieldAmount; i++)
                {
                    droppedItem = ItemPoolManager.Instance.GrabItem(crop.cropYield);
                    droppedItem.transform.position = itemDropTransform.position;
                }
                for(int i = 0; i < crop.seedYieldAmount; i++)
                {
                    droppedItem = ItemPoolManager.Instance.GrabItem(crop.cropSeed);
                    droppedItem.transform.position = itemDropTransform.position;
                }
            }

            crop = null;
            SpriteChange();
        }
    }

    public override void HourPassed()
    {
        if(ignoreNextGrowthMoment || rotted)
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
                CropDied();
            }
            hoursSpent = 0;
            growthStage++;
            if(growthStage == crop.growthStages - 1) harvestable = true;
            SpriteChange();
        }
        if(!rotted)
        {
            //update the struct manager after reducing the nutrition values from the tile
            bool plantDied = false;
            nutrients.ichorLevel -= crop.ichorIntake;
            if(nutrients.ichorLevel < 0)
            {
                nutrients.ichorLevel = 0;
                plantDied = true;
            }
            nutrients.terraLevel -= crop.terraIntake;
            if(nutrients.terraLevel < 0)
            {
                nutrients.terraLevel = 0;
                plantDied = true;
            }
            nutrients.gloamLevel -= crop.gloamIntake;
            if(nutrients.gloamLevel < 0)
            {
                nutrients.gloamLevel = 0;
                plantDied = true;
            }
            waterLevel -= crop.waterIntake;
            if(waterLevel < 0)
            {
                waterLevel = 0;
                plantDied = true;
            }
            StructureManager.Instance.UpdateStorage(transform.position, nutrients);

            if(plantDied)
            {
                CropDied();
            }
        }
    }

    void SpriteChange()
    {
        print(growthStage);
        if(crop) cropRenderer.sprite = crop.cropSprites[(growthStage - 1)];
        else cropRenderer.sprite = null;
    }

    public void CropDied()
    {
        rotted = true;
        harvestable = true;
        growthStage = crop.growthStages;
        SpriteChange();
    }

    public void CropDestroyed()
    {
        crop = null;
        harvestable = false;
        SpriteChange();
    }
}
