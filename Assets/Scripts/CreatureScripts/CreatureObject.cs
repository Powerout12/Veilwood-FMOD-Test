using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature Object", menuName = "Creature")]
public class CreatureObject : ScriptableObject
{
    public GameObject objectPrefab;
    public float health;

    public float[] position = new float[3];

    public int dangerCost = 1;

    public Creature data = new Creature();

    public Creature CreateCreature()
    {
        Creature newCreature = new Creature(this);
        return newCreature;
    }
}
[System.Serializable]
public class Creature
{
    [Header("Variables that need to be saved")]
    public string Name;
    public int Id = -1;
    public float health;
    public float[] position = new float[3];


    public Creature()
    {
        Name = "";
        Id = -1;
        position = new float[3];
    }
    public Creature(CreatureObject Creature)
    {
        Name = Creature.name;
        Id = Creature.data.Id;
    }
}
