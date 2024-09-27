using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static int currentHour = 9; //caps at 24, day is from 6-20. Military time. Night begins at 8PM,(20) and ends at 6AM, lasting 10 hours. Day lasts 14 hours. Each hour lasts 45 seconds
    StructureManager structManager;
    // Start is called before the first frame update
    void Start()
    {
        structManager = GetComponent<StructureManager>();
        StartCoroutine("TimePassage");
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
        }
        while(gameObject.activeSelf);
    }
}
