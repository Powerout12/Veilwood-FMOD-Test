using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
}
