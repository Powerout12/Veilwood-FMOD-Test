using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crow : CreatureBehaviorScript
{

    public float radius = 10f;   // Distance from the player
    public float height = 5f;   // Altitude above the ground
    public float attackHeight = 3f;
    public float circleSpeed = 2f; // Speed of circling
    public bool flyUp;
    private float angle = 0f;   // Current angle on the circular path
    private bool coroutineRunning = false;
    private float timeBeforeAttack;
    private float savedAngle;

    public enum CreatureState
    {
        Idle,
        CirclePlayer,
        AttackPlayer,
        Stun,
        Die,
        Trapped
    }

    public CreatureState currentState;
    private void Start()
    {
        base.Start();
        currentState = CreatureState.Idle;
        timeBeforeAttack = Random.Range(5, 10);
    }



    void Update()
    {
        CheckState(currentState);
    }

    public void CheckState(CreatureState currentState)
    {
        switch (currentState)
        {
            case CreatureState.Idle:
                Idle();
                break;

            case CreatureState.CirclePlayer:
                CircleAroundPlayer();
                break;

            case CreatureState.AttackPlayer:
                AttackPlayer();
                break;

            case CreatureState.Stun:
                Stun();
                break;

            case CreatureState.Die:
                Die();
                break;

            case CreatureState.Trapped:
                Trapped();
                break;

            default:
                Debug.LogError("Unknown state: " + currentState);
                break;
        }
    }

    void CircleAroundPlayer()
    {
       

        angle += circleSpeed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        timeBeforeAttack -= Time.deltaTime;
        if (timeBeforeAttack < 0)
        {
            savedAngle = angle;
            currentState = CreatureState.AttackPlayer;
        }

        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

        if (flyUp) //flys crow up
        {
            height += 0.05f;
            if (height >= 15f)
            {
                flyUp = false;
            }
        }
        else if (!flyUp) //flys crow down
        {
            height -= 0.05f;
            if (height <= 5f)
            {
                flyUp = true;
            }
        }

        Vector3 targetPosition = player.position + offset + Vector3.up * height;


        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * circleSpeed); //move towards direction


        transform.LookAt(targetPosition); //face the direction its flying

    }

    private void Idle()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        playerInSightRange = distance <= sightRange;
        if (playerInSightRange)
        {
            currentState = CreatureState.CirclePlayer;
        }
    }

    private void AttackPlayer()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(Attack());
           
        }
    }

    private IEnumerator Attack()
    {
        coroutineRunning = true;

      
        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position + Vector3.up * attackHeight;

       
        Vector3 directionToPlayer = (transform.position - player.position).normalized;
        savedAngle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x);
        float oppositeAngle = savedAngle + Mathf.PI;

       
        Vector3 offset = new Vector3(Mathf.Cos(oppositeAngle), 0, Mathf.Sin(oppositeAngle)) * radius;
        Vector3 endPos = player.position + offset + Vector3.up * height;

       
        float swoopSpeed = circleSpeed * 10; 
        float rotationSpeed = 5f; 

        // First Phase: Swoop down to the target position
        while (Vector3.Distance(transform.position, targetPos) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, swoopSpeed * Time.deltaTime);

            
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            yield return null; 
        }

        

        // Second Phase: Swoop back up to the end position
        while (Vector3.Distance(transform.position, endPos) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, swoopSpeed * Time.deltaTime);

           
            Quaternion targetRotation = Quaternion.LookRotation(endPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            yield return null; 
        }

        // Reset the angle for the circling phase
        angle = oppositeAngle;
        timeBeforeAttack = Random.Range(5, 10);
        coroutineRunning = false;
        currentState = CreatureState.CirclePlayer;
    }








    private void Stun()
    {
        throw new NotImplementedException();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    private void Trapped()
    {
        throw new NotImplementedException();
    }


}
