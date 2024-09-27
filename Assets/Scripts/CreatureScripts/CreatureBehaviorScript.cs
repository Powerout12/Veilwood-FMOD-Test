using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBehaviorScript : MonoBehaviour
{
    //This is the base class that ALL creatures should derive from
    public float health = 10;
    public float maxHealth = 10;

    StructureManager structManager;

    public float sightRange = 4; //how far can it see the player

    public void Start()
    {
        structManager = FindObjectOfType<StructureManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    
}
