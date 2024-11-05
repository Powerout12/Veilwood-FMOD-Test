using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelAttack : MonoBehaviour
{
    public LayerMask hitDetection;
    public Collider collider;

    void Start()
    {
        collider.enabled = false;
    }
    
    public IEnumerator Swing()
    {
        collider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        collider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        var structure = other.GetComponent<StructureBehaviorScript>();
        if (structure != null)
        {
            structure.health -= 2;
            print("Hit Structure");
            PlayerInteraction.Instance.StaminaChange(-1);
            collider.enabled = false;
        }

        var creature = other.GetComponent<CreatureBehaviorScript>();
        if (creature != null)
        {
            creature.TakeDamage(25);
            //playsound
            print("Hit Creature");
            PlayerInteraction.Instance.StaminaChange(-1);
            collider.enabled = false;
        }

        //Something to hit corpses

        
    }
}
