using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crow : CreatureBehaviorScript
{
    public List<StructureBehaviorScript> availableStructure = new List<StructureBehaviorScript>();
    StructureBehaviorScript scaryStructure;

    public float radius = 10f;   // Distance from the player
    public float height = 5f;    // Altitude above the ground
    public float attackHeight = 3f;
    public float circleSpeed = 2f; // Speed of circling
    private float angle = 0f;    // Current angle on the circular path
    private bool coroutineRunning = false;
    private float timeBeforeAttack;
    private float savedAngle;

    public enum CreatureState
    {
        Idle,
        Flee,
        CirclePlayer,
        AttackPlayer,
        Stun,
        Land,
        Die,
        Trapped
    }

    public CreatureState currentState;

    private void Start()
    {
        base.Start();
        currentState = CreatureState.Idle;
        StructureBehaviorScript.OnStructuresUpdated += UpdateStructureList;
        timeBeforeAttack = Random.Range(5, 10);
        UpdateStructureList();
    }

    private void OnDestroy()
    {
        StructureBehaviorScript.OnStructuresUpdated -= UpdateStructureList;
    }

    private void UpdateStructureList()
    {
        availableStructure.Clear();
        foreach (StructureBehaviorScript structure in structManager.allStructs)
        {
            if (structure is ImbuedScarecrow scarecrow)
            {
                availableStructure.Add(scarecrow);
            }
        }
    }

    private void Update()
    {
        CheckForScarecrowDistance();
        CheckState(currentState);
    }

    private void CheckForScarecrowDistance()
    {
        float shortestDistanceFromScarecrow = float.PositiveInfinity;
        StructureBehaviorScript closestScarecrow = null;

        foreach (var structure in availableStructure)
        {
            float distance = Vector3.Distance(structure.transform.position, transform.position);
            if (distance < shortestDistanceFromScarecrow)
            {
                shortestDistanceFromScarecrow = distance;
                closestScarecrow = structure;
            }
        }

        if (closestScarecrow != null && shortestDistanceFromScarecrow < 10f)
        {
            if (currentState != CreatureState.Flee)
            {
                StopAllCoroutines();
                coroutineRunning = false;
                scaryStructure = closestScarecrow;
                currentState = CreatureState.Flee;
            }
        }
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
            case CreatureState.Flee:
                Flee();
                break;
            case CreatureState.Stun:
                Stun();
                break;
            case CreatureState.Land:
                Land();
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

    private void Land()
    {
       
    }

    private void Flee()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(FleeFromStructure());
        }
    }

    private IEnumerator FleeFromStructure()
    {
        coroutineRunning = true;
        Vector3 fleeDirection = (transform.position - scaryStructure.transform.position).normalized;

        // First Part: 180° turn to face away from the scarecrow
        float angle = 0f;
        Vector3 basePosition = transform.position;
        while (angle < Mathf.PI) // 180° turn in radians (π radians)
        {
            angle += circleSpeed * Time.deltaTime;

            // Calculate offset for the half-circle turn
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius * 2;

            // Target position based on the crow's initial position + offset
            Vector3 targPosition = basePosition + offset;

            // Smoothly move towards target position to complete the turn
            transform.position = Vector3.Lerp(transform.position, targPosition, Time.deltaTime * circleSpeed);

            // Rotate to face away from the scarecrow while turning
            transform.LookAt(transform.position + fleeDirection);

            yield return null;
        }

       

        coroutineRunning = false;

        // Decision Phase: Determine whether to circle or land
        if (Random.value < 0.5f) // 50% chance to circle, 50% chance to land
        {
            currentState = CreatureState.CirclePlayer;
        }
        else
        {
            currentState = CreatureState.Land;
            StartCoroutine(LandOnGround()); // Begin landing coroutine if the crow chooses to land
        }
    }

    // Landing Coroutine
    private IEnumerator LandOnGround()
    {
        Vector3 landingPosition = new Vector3(transform.position.x, 0, transform.position.z); // Ground level
        float landingSpeed = 5f; // Slower descent

        while (Vector3.Distance(transform.position, landingPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, landingPosition, landingSpeed * Time.deltaTime);
            yield return null;
        }

        // Landing complete, transition to idle or another desired state
        currentState = CreatureState.Idle;
    }






    void CircleAroundPlayer()
    {
        // Calculate target circle position
        angle += circleSpeed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        height = Mathf.PingPong(Time.time * 2, 10f) + 5f;
        Vector3 targetPosition = player.position + offset + Vector3.up * height;

        // If the crow is far from the circle position, move towards it smoothly
        if (Vector3.Distance(transform.position, targetPosition) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * circleSpeed);
        }
        else
        {
            // Otherwise, move in a normal circular motion around the player
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * circleSpeed);
        }

        transform.LookAt(targetPosition);
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
        float swoopSpeed = circleSpeed * 10;
        float rotationSpeed = 5f;

        while (Vector3.Distance(transform.position, targetPos) > 1f)
        {
           
            transform.position = Vector3.MoveTowards(transform.position, targetPos, swoopSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        Vector3 directionToPlayer = (transform.position - player.position).normalized;
        savedAngle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x);
        float oppositeAngle = savedAngle + Mathf.PI;
        Vector3 offset = new Vector3(Mathf.Cos(oppositeAngle), 0, Mathf.Sin(oppositeAngle)) * radius;
        Vector3 endPos = player.position + offset + Vector3.up * height;

        while (Vector3.Distance(transform.position, endPos) > 1f)
        {
           
            transform.position = Vector3.MoveTowards(transform.position, endPos, swoopSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(endPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

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
