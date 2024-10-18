using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICropStats : MonoBehaviour
{
    public Camera mainCam;
    public GameObject cropStatsObject, growthStageText;
    public TextMeshProUGUI cropNameText, gloamAmount, terraAmount, ichorAmount, waterAmount, growthStageNumber;
    public float reach = 5;
    public FarmLand hitCrop;
    public string cropName = "Crop Name";
    public float wlHigh, wlMedium; //Water Level Values
    public float nHigh, nMedium;

    // Start is called before the first frame update
    void Start()
    {
        if(!mainCam) mainCam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        FarmlandCheck();
    }

    public void FarmlandCheck()
    {
        Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, fwd, out hit, reach, 1 << 6))
        {
            if (hit.collider.gameObject.tag == "FarmLand")
            {
                cropStatsObject.SetActive(true);
                hitCrop = hit.collider.GetComponent<FarmLand>();
                //print(hitCrop.cropStats.waterLevel);
                if(hitCrop.growthStage < 0)
                {
                    growthStageText.SetActive(false);
                    cropNameText.text = "Farmland";
                }
                else
                {
                    growthStageText.SetActive(true);
                    cropNameText.text = cropName;
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

    void FarmlandStatUpdate(FarmLand crop) //Cam don't look at this
    {
        //Gloam Level Check I feel so gloaming
        if(crop.cropStats.gloamLevel >= nHigh)
        {
            gloamAmount.text = "High";
        }
        else if(crop.cropStats.gloamLevel >= nMedium)
        {
            gloamAmount.text = "Medium";
        }
        else
        {
            gloamAmount.text = "Low";
        }

        //Terra Level Check
        if(crop.cropStats.terraLevel >= nHigh)
        {
            terraAmount.text = "High";
        }
        else if(crop.cropStats.terraLevel >= nMedium)
        {
            terraAmount.text = "Medium";
        }
        else
        {
            terraAmount.text = "Low";
        }

        //Ichor Level Check idk if it's actually called ichor but that's what it says in the structure manager script so that's what I'm going with
        if(crop.cropStats.ichorLevel >= nHigh)
        {
            ichorAmount.text = "High";
        }
        else if(crop.cropStats.ichorLevel >= nMedium)
        {
            ichorAmount.text = "Medium";
        }
        else
        {
            ichorAmount.text = "Low";
        }

        //Water Level Check
        if(crop.cropStats.waterLevel >= wlHigh)
        {
            waterAmount.text = "High";
        }
        else if(crop.cropStats.waterLevel >= wlMedium)
        {
            waterAmount.text = "Medium";
        }
        else
        {
            waterAmount.text = "Low";
        }

        //Growth Stage Check
        growthStageNumber.text = crop.growthStage.ToString();
    }
}
