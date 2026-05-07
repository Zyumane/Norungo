using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuitState : State
{
    public enum PursuitPhase
    {
        Pursuing,
        Searching,
        Alerted, 
        Returning
    }

    [SerializeField] private PursuitPhase currentPhase = PursuitPhase.Pursuing;

    private Vector3 lastKnownPosition;
    [SerializeField] private float searchArrivalThreshold = 1.5f;

    public override State Tick(ZombieManager zombieManager)
    {
        switch(currentPhase)
        {
            case PursuitPhase.Pursuing:
                HandlePursuing(zombieManager);
                break;
            case PursuitPhase.Searching:
                HandleSearching(zombieManager);
                break;
            case PursuitPhase.Returning:
                HandleReturning(zombieManager);
                break;
        }
        Debug.Log("[PURSUIT] Fase actual: " + currentPhase);
        return this;
    }

    private void MoveTorwardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 2, 0.2f, Time.deltaTime);
    }

    private void RotateTowardsTarget(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget == null)
            return;

        if (zombieManager.zombieNavmeshAgent == null)
            return;

        zombieManager.zombieNavmeshAgent.enabled = true;
        zombieManager.zombieNavmeshAgent.SetDestination(zombieManager.currentTarget.transform.position);

        zombieManager.transform.rotation = Quaternion.Slerp(
            zombieManager.transform.rotation,
            zombieManager.zombieNavmeshAgent.transform.rotation,
            zombieManager.rotationSpeed / Time.deltaTime);
    }

    private void HandlePursuing(ZombieManager zombieManager)
    {
        lastKnownPosition = zombieManager.currentTarget.transform.position;
        MoveTorwardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);

        if(zombieManager.zombieIdleState.IsPlayerInLineOfSight(zombieManager))
        {
            lastKnownPosition = zombieManager.currentTarget.transform.position;
            MoveTorwardsCurrentTarget(zombieManager);
            RotateTowardsTarget(zombieManager);
        }
        else
        {
            currentPhase = PursuitPhase.Searching;
        }
    }

    private void HandleSearching(ZombieManager zombieManager)
    {
        if(zombieManager.zombieIdleState.IsPlayerInLineOfSight(zombieManager))
        {
            currentPhase = PursuitPhase.Pursuing;
            return;
        }

        zombieManager.zombieNavmeshAgent.SetDestination(lastKnownPosition);
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
        zombieManager.zombieNavmeshAgent.SetDestination(lastKnownPosition);
        float distanceToLastKnown = Vector3.Distance(zombieManager.transform.position, lastKnownPosition);

        if(distanceToLastKnown <= searchArrivalThreshold)
        {
            zombieManager.currentTarget = null;
            currentPhase = PursuitPhase.Returning;
        }
    }

    private void HandleReturning(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
        zombieManager.zombieIdleState.FindATargetViaLineOfSight(zombieManager);

        if (zombieManager.currentTarget != null)
        {
            currentPhase = PursuitPhase.Pursuing;
        }
    }
}

/*Manual Rollback 
 
    private void RotateTorwardsTarget(ZombieManager zombieManager)
    {
        if(zombieManager.currentTarget == null)
        {
            return;
        }

        zombieManager.zombieNavmeshAgent.enabled = true;
        zombieManager.zombieNavmeshAgent.SetDestination(zombieManager.currentTarget.transform.position);

        zombieManager.transform.rotation = Quaternion.Slerp(
            zombieManager.transform.rotation,
            zombieManager.zombieNavmeshAgent.transform.rotation,
            zombieManager.rotationSpeed / Time.deltaTime);
    }
 
 
 
 
 
 
 */