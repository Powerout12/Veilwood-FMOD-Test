using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : StructureBehaviorScript
{
    public Transform topClamp, bottomClamp;
    //float topClampTarget, bottomClampTarget;
    float animationTimeLeft;
    bool isTriggered, rearming;

    public AudioClip triggeredSFX;

    Vector3 targetAngleTop = new Vector3(-161, 90, -90);
    Vector3 targetAngleBottom = new Vector3(-20, 90, -90);

    Vector3 currentAngleTop;
    Vector3 currentAngleBottom;

    Vector3 startingAngleTop;
    Vector3 startingAngleBottom;

    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        currentAngleTop = topClamp.eulerAngles;
        currentAngleBottom = bottomClamp.eulerAngles;
        startingAngleTop = topClamp.eulerAngles;
        startingAngleBottom = bottomClamp.eulerAngles;
        if(isTriggered)
        {
            topClamp.rotation = Quaternion.Euler(-161, 90, -90);
            bottomClamp.rotation = Quaternion.Euler(-20, 90, -90);
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if(animationTimeLeft > 0)
        {
            currentAngleTop = new Vector3( Mathf.LerpAngle(currentAngleTop.x, targetAngleTop.x, Time.deltaTime * 1f), 90, -90);

            topClamp.eulerAngles = currentAngleTop;

            currentAngleBottom = new Vector3( Mathf.LerpAngle(currentAngleBottom.x, targetAngleBottom.x, Time.deltaTime * 1f), 90, -90);

            bottomClamp.eulerAngles = currentAngleBottom;


            animationTimeLeft -= Time.deltaTime;

        }
    }

    public override void StructureInteraction()
    {
        if(isTriggered && !rearming)
        {
            StartCoroutine(Rearm());
        }
    }

    IEnumerator SpringTrap(Collider victim)
    {
        //stuns an enemy with over 50 hp, otherwise insta kills
        animationTimeLeft = 0.5f;
        yield return new WaitForSeconds(0.5f);
        topClamp.rotation = Quaternion.Euler(-161, 90, -90);
        bottomClamp.rotation = Quaternion.Euler(-20, 90, -90);
        source.PlayOneShot(triggeredSFX);
        //does the damage
        if(victim.GetComponent<PlayerInteraction>())
        {
            PlayerInteraction player = victim.GetComponent<PlayerInteraction>();
            player.PlayerTakeDamage();

            //restrictplayermovement
            PlayerMovement.restrictMovement = true;
            
            yield return new WaitForSeconds(1);
            StartCoroutine(Rearm());

            yield return new WaitForSeconds(0.5f);
            PlayerMovement.restrictMovement = false;
            //enable player movement
        } 
        else
        {
            CreatureBehaviorScript creature = victim.GetComponent<CreatureBehaviorScript>();
            if(creature.health > 50)
            {
                //stun and damage
            }
            else
            {
                //kill
            }
        }
    }

    IEnumerator Rearm()
    {
        float lerp = 0;
        rearming = true;
        do
        {
            lerp += 0.1f;
            currentAngleTop = new Vector3( Mathf.LerpAngle(currentAngleTop.x, startingAngleTop.x, lerp), 90, -90);

            topClamp.eulerAngles = currentAngleTop;

            currentAngleBottom = new Vector3( Mathf.LerpAngle(currentAngleBottom.x, startingAngleBottom.x, lerp), 90, -90);

            bottomClamp.eulerAngles = currentAngleBottom;
            yield return new WaitForSeconds(0.1f);
        }
        while(lerp < 10);
        topClamp.eulerAngles = startingAngleTop;
        bottomClamp.eulerAngles = startingAngleBottom;
        isTriggered = false;
        rearming = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(isTriggered) return;
        if(other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            isTriggered = true;
            StartCoroutine(SpringTrap(other)); //pass enemy script or player script variable
        }
    }
}
