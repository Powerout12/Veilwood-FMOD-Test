using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightSpawningManager : MonoBehaviour
{
    float difficultyPoints = 0;

    public CreatureObject[] creatures;

    public Transform[] testSpawns;

    void Start()
    {
        TimeManager.OnHourlyUpdate += HourUpdate;
        //load old danger values
    }

    void HourUpdate()
    {
        if(TimeManager.isDay)
        {
            return;
        }
        if(TimeManager.currentHour == 20)
        {
            foreach(StructureBehaviorScript structure in StructureManager.Instance.allStructs)
            {
               difficultyPoints += structure.wealthValue;
            }
        }
        SpawnCreatures();
    }

    void SpawnCreatures()
    {
        //temp code for demo
        float r;
        int i;
        Transform t;

        r = Random.Range(0f,10f);
        if(r > 5)
        {
            i = Random.Range(0,2);
            Instantiate(creatures[0].objectPrefab, testSpawns[i].position, Quaternion.identity);
        }

        r = Random.Range(0f,10f);
        if(r > 7.5f)
        {
            i = Random.Range(0,2);
            Instantiate(creatures[1].objectPrefab, testSpawns[i].position, Quaternion.identity);
        }
    }
}
