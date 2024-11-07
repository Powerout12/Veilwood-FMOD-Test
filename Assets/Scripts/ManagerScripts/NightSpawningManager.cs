using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightSpawningManager : MonoBehaviour
{
    float difficultyPoints = 0;
    //float originalDifficultyPoints = 0;

    public CreatureObject[] creatures;
    List<int> spawnedCreatures; //tracks how many of a specific type of creature was spawned this hour

    public float[] spawnThresholds; //each hour the difficulty points should drop to this times the original

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
            //difficultyPoints += TimeManager.dayNum;
            //originalDifficultyPoints = difficultyPoints;
        }
        //HourlySpawns();
    }

    void HourlySpawns()
    {
        List<int> weightArray = new List<int>();
        spawnedCreatures = new List<int>();
        int w = 0;
        foreach(CreatureObject c in creatures)
        {
            if(c.dangerThreshold <= difficultyPoints)
            {
                for(int s = 0; s < c.spawnWeight; s++) weightArray.Add(w);
            }
            w++;
        }

        //try to spawn up to 15 things per hour, with a failed appempt counting for 0.5f tries
        float l = 0;
        int r;
        do
        {
            r = Random.Range(0, weightArray.Count);
            CreatureObject attemptedCreature = creatures[weightArray[r]];
            if(attemptedCreature.dangerCost <= difficultyPoints && spawnedCreatures[weightArray[r]] < attemptedCreature.spawnCap)
            {
                spawnedCreatures[weightArray[r]]++;
                l++;
            }
            else l += 0.5f;
            
        }
        while(l < 10); //add threshhold req too
    }

    void SpawnCreature(CreatureObject c)
    {
        int t = Random.Range(0,testSpawns.Length);
        GameObject newCreature = Instantiate(c.objectPrefab, testSpawns[t].position, Quaternion.identity);
        if(newCreature.TryGetComponent<CreatureBehaviorScript>(out var enemy))
        {
            enemy.OnSpawn();
        }
    }
}
