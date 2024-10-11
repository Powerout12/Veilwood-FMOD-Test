using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : StructureBehaviorScript
{
    public Animator anim;
    bool isTriggered;

    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    void SpringTrap()
    {
        //stuns an enemy with over 50 hp, otherwise insta kills
    }
}
