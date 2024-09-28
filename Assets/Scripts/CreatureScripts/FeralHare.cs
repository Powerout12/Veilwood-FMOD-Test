using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeralHare : CreatureBehaviorScript
{
    public CropData[] desiredCrops; // what crops does this creature want to eat

    bool isFleeing = false;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void CropCheck()
    {
        //search for crops
    }

    public void Hop()
    {
        //hare will jump toward a random direction using physics, using rb.addforce to a random vector3 position in addition to a vector3.up force
    }
}
