using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FarmLand : StructureBehaviorScript
{
    public CropData crop; //The current crop planted here
    public InventoryItemData terraFert, gloamFert, ichorFert;
    public SpriteRenderer cropRenderer;
    public Transform itemDropTransform;
    public Collider finishedGrowingCollider;

    public MeshRenderer meshRenderer;
    public Material dry, wet, barren, barrenWet;

    //public float nutrients.waterLevel; //How much has this crop been watered
    public int growthStage = -1; //-1 means there is no crop
    public int hoursSpent = 0; //how long has the plant been in this growth stage for?
    int plantStress = 0; //how much stress the plant has, gained from lack of nutrients/water. If 0 stress, the plant can produce seeds

    public bool harvestable = false; //true if growth stage matches crop data growth stages
    public bool rotted = false;
    public bool isWeed = false;
    bool forceDig = false;

    private bool ignoreNextGrowthMoment = false; //tick this if crop was just planted

    PlayerInventoryHolder playerInventoryHolder;

    private NutrientStorage nutrients;

    public VisualEffect growth, growthComplete, waterSplash, ichorSplash;
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        if(growth) growth.Stop();
        if(growthComplete) growthComplete.Stop();

        if(!crop) wealthValue = 0;
    }

    void Start()
    {
        ParticlePoolManager.Instance.MoveAndPlayParticle(transform.position, ParticlePoolManager.Instance.dirtParticle);
        if (!crop) ignoreNextGrowthMoment = true;
        else if(crop.harvestableGrowthStages.Contains(growthStage))
        {
            harvestable = true;
        }
        playerInventoryHolder = FindObjectOfType<PlayerInventoryHolder>();

        nutrients = StructureManager.Instance.FetchNutrient(transform.position);

        if(isWeed)
        {
            growthStage = Random.Range(0, crop.growthStages);
            growthStage++;
        }

        waterSplash.Stop();
        ichorSplash.Stop();

        SpriteChange();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if(((crop && growthStage >= crop.growthStages) || isWeed) && !finishedGrowingCollider.enabled) finishedGrowingCollider.enabled = true;

        if((!crop || growthStage < crop.growthStages) && finishedGrowingCollider.enabled) finishedGrowingCollider.enabled = false;
    }

    public override void ItemInteraction(InventoryItemData item)
    {
        if(item == terraFert && nutrients.terraLevel < 10)
        {
            nutrients.terraLevel = 10;
            HotbarDisplay.currentSlot.AssignedInventorySlot.RemoveFromStack(1);
            playerInventoryHolder.UpdateInventory();
            return;
        }
        if(item == gloamFert && nutrients.gloamLevel < 10)
        {
            nutrients.gloamLevel = 10;
            HotbarDisplay.currentSlot.AssignedInventorySlot.RemoveFromStack(1);
            playerInventoryHolder.UpdateInventory();
            return;
        }
        if(item == ichorFert && nutrients.ichorLevel < 10)
        {
            nutrients.ichorLevel = 10;
            HotbarDisplay.currentSlot.AssignedInventorySlot.RemoveFromStack(1);
            playerInventoryHolder.UpdateInventory();
            return;
        }

        if(crop) return;
        CropItem newCrop = item as CropItem;
        if(newCrop && newCrop.plantable)
        {
            crop = newCrop.cropData;
            growthStage = 1;
            hoursSpent = 0;
            plantStress = 0;
            SpriteChange();
            HotbarDisplay.currentSlot.AssignedInventorySlot.RemoveFromStack(1);
            playerInventoryHolder.UpdateInventory();
            ParticlePoolManager.Instance.MoveAndPlayParticle(transform.position, ParticlePoolManager.Instance.dirtParticle);

            audioHandler.PlayRandomSound(audioHandler.miscSounds1);

            wealthValue = 5;

        }
    }

    public override void StructureInteraction()
    {
        if(harvestable || forceDig)
        {
            audioHandler.PlaySound(audioHandler.interactSound);
            harvestable = false;
            forceDig = false;
            if(rotted == false)
            {
                if (crop.creaturePrefab)
                {
                    Instantiate(crop.creaturePrefab, transform.position, transform.rotation); //Code needs work once mandrake crop is added
                }
                else
                {
                    GameObject droppedItem;
                    for (int i = 0; i < crop.cropYieldAmount; i++)
                    {
                        droppedItem = ItemPoolManager.Instance.GrabItem(crop.cropYield);
                        droppedItem.transform.position = transform.position;
                    }
                    int r = Random.Range(crop.seedYieldAmount - crop.seedYieldVariance, crop.seedYieldAmount + crop.seedYieldVariance + 1);
                    if(r == 0) r = 1;
                    for (int i = 0; i < r; i++)
                    {
                        if(crop.cropSeed && plantStress == 0)
                        {
                            droppedItem = ItemPoolManager.Instance.GrabItem(crop.cropSeed);
                            droppedItem.transform.position = transform.position;
                        }
                        
                    }
                    
                }
            }

            crop = null;
            hoursSpent = 0;
            if (isWeed) Destroy(this.gameObject);
            SpriteChange();
        }
    }

    public override void ToolInteraction(ToolType type, out bool success)
    {
        success = false;
        if(type == ToolType.Shovel && !forceDig && crop)
        {
            StartCoroutine(DigPlant());
            success = true;
        }
        if(type == ToolType.WateringCan && PlayerInteraction.Instance.waterHeld > 0 && nutrients.waterLevel < 10)
        {
            PlayerInteraction.Instance.waterHeld--;
            nutrients.waterLevel = 10;
            SpriteChange();
            success = true;

            waterSplash.Play();
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
        crop.OnHour(this);

        if(hoursSpent >= crop.hoursPerStage || StructureManager.Instance.ignoreCropGrowthTime)
        {
            if(growthStage >= crop.growthStages)
            {
                //IT HAS REACHED MAX GROWTH STATE

                //if(hoursSpent < crop.hoursPerStage * 3) return;
                //plant rots
                //CropDied();
            }
            else
            {
                hoursSpent = 0;
                if(!isWeed)
                {
                    growthStage++;
                    if(growth) growth.Play();
                }
                if(crop.harvestableGrowthStages.Contains(growthStage))
                {
                    harvestable = true;
                    if(growth) growth.Stop();
                    if(growthComplete) growthComplete.Play();
                }
                else harvestable = false;
                SpriteChange();
            }
            
        }
        else return;

        DrainNutrients();
    }

    public void InsertCreature(CropData _data, int _growthStage)
    {
        //the mimic will use this function to "plant" itself
        isWeed = true;
        crop = _data;
        growthStage = _growthStage;
        SpriteChange();
    }

    public void SpriteChange()
    {
        if(crop) 
        {
            if(rotted) cropRenderer.sprite = crop.rottedImage;
            else cropRenderer.sprite = crop.cropSprites[(growthStage - 1)];
        }
        else cropRenderer.sprite = null;

        if(nutrients.ichorLevel <= 1 || nutrients.terraLevel <= 1 || nutrients.gloamLevel <= 1)
        {
            meshRenderer.material = barren;
        }
        else meshRenderer.material = dry;

        if(nutrients.waterLevel > 5)
        {
            if(meshRenderer.material == barren) meshRenderer.material = barrenWet;
            else meshRenderer.material = wet;
        }
    }

    void DrainNutrients()
    {
        //PLANTS DRAIN PER GROWTH STAGE, AND THE PLAYER SHOULD HAVE TO WATER ROUGHLY EVERY STAGE/EVERY OTHER STAGE
        nutrients.ichorLevel -= crop.ichorIntake;
        if(nutrients.ichorLevel < 0)
        {
            nutrients.ichorLevel = 0;
            plantStress++;
        }
        else if(nutrients.ichorLevel > 10) nutrients.ichorLevel = 10;

        nutrients.terraLevel -= crop.terraIntake;
        if(nutrients.terraLevel < 0)
        {
            nutrients.terraLevel = 0;
            plantStress++;
        }
        else if(nutrients.terraLevel > 10) nutrients.terraLevel = 10;

        nutrients.gloamLevel -= crop.gloamIntake;
        if(nutrients.gloamLevel < 0)
        {
            nutrients.gloamLevel = 0;
            plantStress++;
        }
        else if(nutrients.gloamLevel > 10) nutrients.gloamLevel = 10;

        nutrients.waterLevel -= crop.waterIntake;
        if(nutrients.waterLevel < 0)
        {
            nutrients.waterLevel = 0;
            plantStress++;
        }
        StructureManager.Instance.UpdateStorage(transform.position, nutrients);

        if(plantStress > crop.stressLimit && !isWeed)
        {
            CropDied();
        }
        else SpriteChange();
    }

    void CropDied()
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
        ParticlePoolManager.Instance.MoveAndPlayParticle(transform.position, ParticlePoolManager.Instance.dirtParticle);
    }

    IEnumerator DigPlant()
    {
        forceDig = true;
        yield return new WaitForSeconds(1f);
        StructureInteraction();
    }

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return; 
        if (crop != null && crop.creaturePrefab)
        {
            Instantiate(crop.creaturePrefab, transform.position, transform.rotation); //Code needs work once mandrake crop is added
        }
        ParticlePoolManager.Instance.MoveAndPlayParticle(transform.position, ParticlePoolManager.Instance.dirtParticle);
        base.OnDestroy();
    }

    public override void TimeLapse(int hours)
    {
        for(int i = 0; i < hours; i++)
        {
            HourPassed();
        }
    }

    public NutrientStorage GetCropStats()
    {
        return nutrients;
    }
}
