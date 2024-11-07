using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightSpawningManager : MonoBehaviour
{
    float difficultyPoints = 0;
    //float originalDifficultyPoints = 0;

    public CreatureObject[] creatures;
    List<int> spawnedCreatures; //tracks how many of a specific type of creature was spawned this hour

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
            print("Tally");
            print(StructureManager.Instance);
            print(StructureManager.Instance.allStructs.Count);
            //WHY IS THIS RETURNING EMPTY?????? WHENEVER THE MISTWALKER DESTORYS A STRUCT, IT EMPTIES THE LIST
            foreach(StructureBehaviorScript structure in StructureManager.Instance.allStructs)
            {
               difficultyPoints += structure.wealthValue;
            }
            //difficultyPoints += TimeManager.dayNum;
            //originalDifficultyPoints = difficultyPoints;
        }
        if(difficultyPoints > 0) HourlySpawns();
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

        //try to spawn up to 10 things per hour, with a failed attempt counting for 0.5f tries
        float l = 0;
        int r;
        float threshhold = difficultyPoints * GetThreshold();
        do
        {
            r = Random.Range(0, weightArray.Count);
            CreatureObject attemptedCreature = creatures[weightArray[r]];
            if(attemptedCreature.dangerCost <= difficultyPoints && spawnedCreatures[weightArray[r]] < attemptedCreature.spawnCap)
            {
                spawnedCreatures[weightArray[r]]++;
                SpawnCreature(attemptedCreature);
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

    float GetThreshold()
    {
        switch (TimeManager.currentHour)
            {
                case 1:
                    return 0.2f;
                    break;
                case 2:
                    return 0.2f;
                    break;
                case 3:
                    return 0;
                    break;
                case 4:
                    return 0;
                    break;
                case 5:
                    return 0;
                    break;
                case 6:
                    return 0;
                    break;
                case 20:
                    return 0.9f;
                    break;
                case 21:
                    return 0.7f;
                    break;
                case 22:
                    return 0.7f;
                    break;
                case 23:
                    return 0.4f;
                    break;
                case 0:
                    return 0.4f;
                    break;
                default:
                    return 1;
                    break;
            }
    }
}
