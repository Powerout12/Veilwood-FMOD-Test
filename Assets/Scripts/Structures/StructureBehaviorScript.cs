using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBehaviorScript : MonoBehaviour
{
    //This is the base class that ALL structures should derive from
    StructureManager structManager;

    public StructureObject structData;

    public float health = 5;
    public float maxHealth = 5;

    // Start is called before the first frame update
    void Start()
    {
        structManager = FindObjectOfType<StructureManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if(!gameObject.scene.isLoaded) return;
        print("Destroyed");
        structManager.ClearTile(transform.position);
    }
}
