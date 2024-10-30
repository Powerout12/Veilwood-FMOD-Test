using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICropStats : MonoBehaviour
{
    public Camera mainCam;
    public GameObject cropStatsObject, growthStageText;
    public TextMeshProUGUI cropNameText, gloamAmount, terraAmount, ichorAmount, waterAmount, growthStageNumber;
    public float reach = 5;
    private FarmLand hitCrop;
    private string cropName = "Crop Name";
    public float wlHigh, wlMedium; //Water Level Values
    public float nHigh, nMedium;

    public Image gloamBG, terraBG, ichorBG;
    public Color c_default, c_rising, c_lowering;

    // Start is called before the first frame update
    void Start()
    {
        if(!mainCam) mainCam = FindObjectOfType<Camera>();
        StartCoroutine(CheckTimer());
    }

    // Update is called once per frame
    void Update()
    {
        //if holding shift, bring detailed info up for crops
    }

    IEnumerator CheckTimer()
    {
        do
        {
            FarmlandCheck();
            yield return new WaitForSeconds(0.2f);
        }
        while(gameObject.activeSelf);
    }

    void FarmlandCheck()
    {
        Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, fwd, out hit, reach, 1 << 6))
        {
            if (hit.collider.gameObject.tag == "FarmLand")
            {
                cropStatsObject.SetActive(true);
                hitCrop = hit.collider.GetComponent<FarmLand>();

                if(hitCrop.growthStage < 0 || !hitCrop.crop)
                {
                    growthStageText.SetActive(false);
                    cropNameText.text = "Farmland";
                }
                else
                {
                    growthStageText.SetActive(true);
                    cropNameText.text = hitCrop.crop.name;
                }
                FarmlandStatUpdate(hitCrop);
            }
            else
            {
                cropStatsObject.SetActive(false);
            }
        }
        else
        {
            cropStatsObject.SetActive(false);
        }
    }

    void FarmlandStatUpdate(FarmLand tile) //Cam don't look at this. //I looked at it
    {
        NutrientStorage tileNutrients = tile.GetCropStats();

        //Gloam Level Check I feel so gloaming
        if(tileNutrients.gloamLevel >= nHigh)
        {
            gloamAmount.text = "High";
        }
        else if(tileNutrients.gloamLevel >= nMedium)
        {
            gloamAmount.text = "Medium";
        }
        else if(tileNutrients.gloamLevel == 0)
        {
            gloamAmount.text = "Depleted";
        }
        else
        {
            gloamAmount.text = "Low";
        }

        //Terra Level Check
        if(tileNutrients.terraLevel >= nHigh)
        {
            terraAmount.text = "High";
        }
        else if(tileNutrients.terraLevel >= nMedium)
        {
            terraAmount.text = "Medium";
        }
        else if(tileNutrients.terraLevel == 0)
        {
            terraAmount.text = "Depleted";
        }
        else
        {
            terraAmount.text = "Low";
        }

        //Ichor Level Check idk if it's actually called ichor but that's what it says in the structure manager script so that's what I'm going with
        if(tileNutrients.ichorLevel >= nHigh)
        {
            ichorAmount.text = "High";
        }
        else if(tileNutrients.ichorLevel >= nMedium)
        {
            ichorAmount.text = "Medium";
        }
        else if(tileNutrients.ichorLevel == 0)
        {
            ichorAmount.text = "Depleted";
        }
        else
        {
            ichorAmount.text = "Low";
        }

        //Water Level Check
        if(tileNutrients.waterLevel >= wlHigh)
        {
            waterAmount.text = "High";
        }
        else if(tileNutrients.waterLevel >= wlMedium)
        {
            waterAmount.text = "Medium";
        }
        else if(tileNutrients.waterLevel == 0)
        {
            waterAmount.text = "Drained";
        }
        else
        {
            waterAmount.text = "Low";
        }

        //Growth Stage Check
        if(tile.isWeed == false && tile.crop)
        {
            string growthString = "Stage: " + tile.growthStage + "/" + tile.crop.growthStages;
            growthStageNumber.text = growthString;
        } 
        else growthStageNumber.text = "";

        if(!tile.crop)
        {
            gloamBG.color = c_default;
            terraBG.color = c_default;
            ichorBG.color = c_default;
            return;
        } 

        if(tile.crop.gloamIntake > 0) gloamBG.color = c_lowering;
        else if(tile.crop.gloamIntake < 0) gloamBG.color = c_rising;
        else gloamBG.color = c_default;

        if(tile.crop.terraIntake > 0) terraBG.color = c_lowering;
        else if(tile.crop.terraIntake < 0) terraBG.color = c_rising;
        else terraBG.color = c_default;

        if(tile.crop.ichorIntake > 0) ichorBG.color = c_lowering;
        else if(tile.crop.ichorIntake < 0) ichorBG.color = c_rising;
        else ichorBG.color = c_default;
    }
}
