using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static int currentHour = 5; //caps at 24, day is from 6-20. Military time. Night begins at 8PM,(20) and ends at 6AM, lasting 10 hours. Day lasts 14 hours. Each hour lasts 45 seconds
    StructureManager structManager;

    public Material skyMat;
    float desiredBlend;
    // Start is called before the first frame update
    void Start()
    {
        structManager = GetComponent<StructureManager>();
        StartCoroutine("TimePassage");
        InitializeSkyBox();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            if(Time.timeScale == 1) Time.timeScale = 8;
            else Time.timeScale = 1;
        }
    }

    IEnumerator TimePassage()
    {
        do
        {
            yield return new WaitForSeconds(45);
            currentHour++;
            if(currentHour >= 24) currentHour = 0;
            structManager.HourUpdate();
            print("Hour passed. Time is now " + currentHour);

            switch (currentHour)
            {
                case 6:
                    SetSkyBox(0.2f);
                    break;
                case 7:
                    SetSkyBox(0.4f);
                    break;
                case 8:
                    SetSkyBox(1f);
                    break;
                case 18:
                    SetSkyBox(0.4f);
                    break;
                case 19:
                    SetSkyBox(0.2f);
                    break;
                case 20:
                    SetSkyBox(0f);
                    break;
                default:
                    //
                    break;
            }

        }
        while(gameObject.activeSelf);
    }

    void SetSkyBox(float b)
    {
        desiredBlend = b;
        StartCoroutine("SkyColorLerp");
    }

    IEnumerator SkyColorLerp()
    {
        float newValue;
        do
        {
            yield return new WaitForSeconds(0.5f);
            newValue = skyMat.GetFloat("_BlendCubemaps");
            if(newValue < desiredBlend) newValue += 0.01f;
            else newValue -= 0.01f;

            newValue = Mathf.Round(newValue * 100f) / 100f;
            print(newValue);
            skyMat.SetFloat("_BlendCubemaps", newValue);
        }
        while(skyMat.GetFloat("_BlendCubemaps") != desiredBlend);
    }

    void InitializeSkyBox()
    {
        if(currentHour < 6 || currentHour >= 20)
        {
            skyMat.SetFloat("_BlendCubemaps", 0f);
            return;
        }
        if(currentHour >= 8 && currentHour < 18)
        {
            skyMat.SetFloat("_BlendCubemaps", 1f);
            return;
        }
        switch (currentHour)
            {
                case 6:
                    skyMat.SetFloat("_BlendCubemaps", 0.2f);
                    break;
                case 7:
                    skyMat.SetFloat("_BlendCubemaps", 0.4f);
                    break;
                case 8:
                    skyMat.SetFloat("_BlendCubemaps", 1f);
                    break;
                case 18:
                    skyMat.SetFloat("_BlendCubemaps", 0.4f);
                    break;
                case 19:
                    skyMat.SetFloat("_BlendCubemaps", 0.2f);
                    break;
                case 20:
                    skyMat.SetFloat("_BlendCubemaps", 0f);
                    break;
                default:
                    //
                    break;
            }
    }
}
