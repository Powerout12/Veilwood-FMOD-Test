using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crow : CreatureBehaviorScript
{
    #region Enums
    public enum CreatureState
    {
        Idle,
        Flee,
        CirclePlayer,
        AttackPlayer,
        Stun,
        Land,
        Die,
        Trapped,
        Wait
    }
    #endregion

    #region Variables
    public List<StructureBehaviorScript> availableStructure = new List<StructureBehaviorScript>();
    private StructureBehaviorScript scaryStructure;

    public float radius = 10f;   
    public float height = 5f;    
    public float attackHeight = 3f;
    public float circleSpeed = 2f; 
    private float angle = 0f;   
    private bool coroutineRunning = false;
    private float timeBeforeAttack;
    private float savedAngle;
    public CreatureState currentState;
    #endregion

    #region Unity Methods
    private void Start()
    {
        base.Start();
        currentState = CreatureState.Idle;
        timeBeforeAttack = Random.Range(5, 10);
    }

    private void OnDestroy()
    {
        StructureBehaviorScript.OnStructuresUpdated -= UpdateStructureList;
    }

    private void Update()
    {
        CheckState(currentState);
    }
    #endregion

    #region State Checking
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
            case CreatureState.Wait:
                Wait();
                break;
            default:
                Debug.LogError("Unknown state: " + currentState);
                break;
        }
    }
    #endregion

    #region State Functions
    private void Idle()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        playerInSightRange = distance <= sightRange;
        if (playerInSightRange)
        {
            currentState = CreatureState.CirclePlayer;
        }
    }

    private void CircleAroundPlayer()
    {
  
        angle += circleSpeed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        height = Mathf.PingPong(Time.time * 2, 10f) + 5f;
        Vector3 targetPosition = player.position + offset + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * circleSpeed);
        transform.LookAt(targetPosition);
        if (!coroutineRunning)
        {
            StartCoroutine(Decide());
        }
    }

    private void AttackPlayer()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(Attack());
        }
    }

    private void Flee()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(FleeFromStructure());
        }
    }

    private void Stun()
    {
        throw new NotImplementedException();
    }

    private void Land()
    {
      
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    private void Trapped()
    {
        throw new NotImplementedException();
    }

    private void Wait() { }

   
    #endregion

    #region Helper Functions
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("scarecrow"))
        {
            StopAllCoroutines();
            coroutineRunning = false;
            currentState = CreatureState.Flee;
        }
    }

    private void ReturnToCircle()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(ReturnToCirclePosition());
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator FleeFromStructure()
    {
        coroutineRunning = true;
        Vector3 targetPosition = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z + 10);

        while (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * circleSpeed);
            yield return null;
        }

        currentState = CreatureState.Wait;
        coroutineRunning = false;
        ReturnToCircle();
    }

    

    private IEnumerator ReturnToCirclePosition()
    {
        coroutineRunning = true;

      
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        Vector3 targetPosition = player.position + offset + Vector3.up * height;

        float distance = Vector3.Distance(transform.position, targetPosition);
        float remainingDistance = distance;

        while (remainingDistance > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1 - (remainingDistance / distance));
            remainingDistance -= circleSpeed * Time.deltaTime;
            yield return null;
        }

        currentState = CreatureState.CirclePlayer;
        coroutineRunning = false;
    }

    private IEnumerator Attack()
    {
        coroutineRunning = true;
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

    private IEnumerator Decide()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(3);
        int x = Random.Range(0, 3);
        if (x == 0) { currentState = CreatureState.AttackPlayer; }
        coroutineRunning = false;
    }
    #endregion
}
