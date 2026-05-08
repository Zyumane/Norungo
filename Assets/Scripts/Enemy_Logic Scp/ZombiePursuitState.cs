using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuitState : State
{
    private Vector3 lastKnownPosition;

    [Header("Searching")]
    [SerializeField] private float searchArrivalThreshold = 1.5f;
    [SerializeField] private float inspectionDuration = 5f;
    [SerializeField] private float sweepAngle = 60f;
    [SerializeField] private float sweepSpeed = 1f;

    [Header("Returning")]
    [SerializeField] private float returnArrivalThreshold = 1.5f;

    private bool isInspecting = false;
    private float inspectionTimer = 0f;
    private Quaternion inspectionBaseRotation;
    private PatrolNode returnTargetNode = null;

    public override State Tick(ZombieManager zombieManager)
    {
        switch (zombieManager.currentPhase)
        {
            case ZombieManager.RenegatePhase.Chasing:
                HandleChasing(zombieManager);
                break;

            case ZombieManager.RenegatePhase.Searching:
                HandleSearching(zombieManager);
                break;

            case ZombieManager.RenegatePhase.Returning:
                bool returnToIdle = HandleReturning(zombieManager);
                if (returnToIdle)
                {
                    zombieManager.currentPhase = ZombieManager.RenegatePhase.Patrolling;
                    return zombieManager.startingState;
                }
                break;
        }

        return this;
    }

    // --- Chasing ---

    private void HandleChasing(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget == null)
        {
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Searching;
            return;
        }

        lastKnownPosition = zombieManager.currentTarget.transform.position;

        if (zombieManager.zombieIdleState.IsPlayerInLineOfSight(zombieManager))
        {
            lastKnownPosition = zombieManager.currentTarget.transform.position;
            MoveTowardsTarget(zombieManager);
            RotateTowardsTarget(zombieManager);
        }
        else
        {
            // Perdio al jugador -- iniciar busqueda con estado limpio
            isInspecting = false;
            inspectionTimer = 0f;
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Searching;
        }
    }

    private void MoveTowardsTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 2, 0.2f, Time.deltaTime);
    }

    private void RotateTowardsTarget(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget == null) return;
        if (zombieManager.zombieNavmeshAgent == null) return;

        zombieManager.zombieNavmeshAgent.enabled = true;
        zombieManager.zombieNavmeshAgent.SetDestination(
            zombieManager.currentTarget.transform.position);

        zombieManager.transform.rotation = Quaternion.Slerp(
            zombieManager.transform.rotation,
            zombieManager.zombieNavmeshAgent.transform.rotation,
            zombieManager.rotationSpeed / Time.deltaTime);
    }

    // --- Searching ---

    private void HandleSearching(ZombieManager zombieManager)
    {
        // Redeteccion -- volver a Chasing inmediatamente
        if (zombieManager.zombieIdleState.IsPlayerInLineOfSight(zombieManager))
        {
            isInspecting = false;
            inspectionTimer = 0f;
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
            return;
        }

        float distanceToLastKnown = Vector3.Distance(
            zombieManager.transform.position, lastKnownPosition);

        // Todavia viajando al ultimo punto visto
        if (!isInspecting && distanceToLastKnown > searchArrivalThreshold)
        {
            zombieManager.zombieNavmeshAgent.SetDestination(lastKnownPosition);
            zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            return;
        }

        // Llego -- iniciar inspeccion
        if (!isInspecting)
        {
            isInspecting = true;
            inspectionTimer = 0f;
            inspectionBaseRotation = zombieManager.transform.rotation;
            zombieManager.zombieNavmeshAgent.SetDestination(
                zombieManager.transform.position);
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
        }

        // Barrido izquierda-derecha (dos ciclos en inspectionDuration segundos)
        inspectionTimer += Time.deltaTime;
        float sweepProgress = inspectionTimer / inspectionDuration;
        float sweepValue = Mathf.Sin(sweepProgress * Mathf.PI * 4f * sweepSpeed);
        float currentSweepAngle = sweepValue * sweepAngle;

        zombieManager.transform.rotation = inspectionBaseRotation *
            Quaternion.Euler(0f, currentSweepAngle, 0f);

        // Inspeccion terminada sin deteccion
        if (inspectionTimer >= inspectionDuration)
        {
            isInspecting = false;
            inspectionTimer = 0f;
            zombieManager.currentTarget = null;
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Returning;
        }
    }

    // --- Returning ---

    private bool HandleReturning(ZombieManager zombieManager)
    {
        if (zombieManager.patrolGraph == null)
            return false;

        if (returnTargetNode == null)
            returnTargetNode = zombieManager.patrolGraph.GetNearestNode(
                zombieManager.transform.position);

        zombieManager.zombieNavmeshAgent.SetDestination(
            returnTargetNode.transform.position);
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

        // Redeteccion durante el regreso
        zombieManager.zombieIdleState.FindATargetViaLineOfSight(zombieManager);
        if (zombieManager.currentTarget != null)
        {
            returnTargetNode = null;
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
            return false;
        }

        bool arrived = !zombieManager.zombieNavmeshAgent.pathPending
                       && zombieManager.zombieNavmeshAgent.remainingDistance
                       <= returnArrivalThreshold;

        if (arrived)
        {
            zombieManager.zombieIdleState.ResetPatrol();
            returnTargetNode = null;
            return true;
        }

        return false;
    }
}

/*Major Manual Rollback
    private Vector3 lastKnownPosition;
    [SerializeField] private float searchArrivalThreshold = 1.5f;

    private PatrolNode returnTargetNode = null;
    [SerializeField] private float returnArrivalThreshold = 1.5f;

    [Header("Searching")]
    [SerializeField] private float inspectionDuration = 5f;
    [SerializeField] private float sweepAngle = 60f;
    [SerializeField] private float sweepSpeed = 1f;

    private bool isInspecting = false;
    private float inspectionTimer = 0f;
    private Quaternion inspectionBaseRotation;

    public override State Tick(ZombieManager zombieManager)
    {
        switch(zombieManager.currentPhase)
        {
            case ZombieManager.RenegatePhase.Chasing:
                HandlePursuing(zombieManager);
                break;
            case ZombieManager.RenegatePhase.Searching:
                HandleSearching(zombieManager);
                break;
            case ZombieManager.RenegatePhase.Returning:
                bool shouldReturnToIdle = HandleReturning_O(zombieManager);
                if (shouldReturnToIdle) 
                {
                    zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
                    return zombieManager.startingState;
                }
                break;
        }
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

        if (zombieManager.zombieIdleState.IsPlayerInLineOfSight(zombieManager))
        {
            // Repite lo mismo — código redundante
            lastKnownPosition = zombieManager.currentTarget.transform.position;
            MoveTorwardsCurrentTarget(zombieManager);
            RotateTowardsTarget(zombieManager);
        }
        else
        {
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Searching;
        }
    }

    private void HandleSearching(ZombieManager zombieManager)
    {
        if(zombieManager.zombieIdleState.IsPlayerInLineOfSight(zombieManager))
        {
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
            return;
        }

        //if(zombieManager.zombieIdleState.IsPlayerInLineOfSight(zombieManager))
        //{
        //    isInspecting = false;
        //    inspectionTimer = 0f;
        //    zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
        //    return;
        //}

        float distanceToLastKnown = Vector3.Distance(zombieManager.transform.position, lastKnownPosition);

        if(!isInspecting && distanceToLastKnown > searchArrivalThreshold)
        {
            zombieManager.zombieNavmeshAgent.SetDestination(lastKnownPosition); 
            zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            return;
        }

        if (!isInspecting)
        {
            // Llego — iniciar inspeccion
            isInspecting = true;
            inspectionTimer = 0f;
            inspectionBaseRotation = zombieManager.transform.rotation;
            zombieManager.zombieNavmeshAgent.SetDestination(zombieManager.transform.position);
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            Debug.Log("[SEARCHING] Iniciando inspección");
        }

        // Barrido Visual
        inspectionTimer += Time.deltaTime;
        Debug.Log("[SEARCHING] Timer: " + inspectionTimer.ToString("F1") + " / " + inspectionDuration);
        float sweepProgress = inspectionTimer / inspectionDuration;

        // Doble ciclo de inspeccion
        float sweepValue = Mathf.Sin(sweepProgress * Mathf.PI * 4f * sweepSpeed);
        float currentSweepAngle = sweepValue * sweepAngle;

        zombieManager.transform.rotation = inspectionBaseRotation * Quaternion.Euler(0f, currentSweepAngle, 0f);

        if (inspectionTimer >= inspectionDuration)
        {
            isInspecting = false;
            inspectionTimer = 0f;
            zombieManager.currentTarget = null;
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Returning;
        }

        if(distanceToLastKnown <= searchArrivalThreshold)
        {
            zombieManager.currentTarget = null;
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Returning;
        }
    }
    
    private bool HandleReturning_O(ZombieManager zombieManager)
    {
        if (zombieManager.patrolGraph == null)
            return false;

        if (returnTargetNode == null)
            returnTargetNode = zombieManager.patrolGraph.GetNearestNode(
                zombieManager.transform.position);

        zombieManager.zombieNavmeshAgent.SetDestination(returnTargetNode.transform.position);
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

        // Redetección durante el regreso
        zombieManager.zombieIdleState.FindATargetViaLineOfSight(zombieManager);
        if (zombieManager.currentTarget != null)
        {
            returnTargetNode = null;
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
            return false;
        }

        bool arrived = !zombieManager.zombieNavmeshAgent.pathPending
                       && zombieManager.zombieNavmeshAgent.remainingDistance <= returnArrivalThreshold;

        if (arrived)
        {
            zombieManager.zombieIdleState.ResetPatrol();
            returnTargetNode = null;
            return true;
        }

        return false;
    }
 
 */

/*Manual Rollback 
 
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
        zombieManager.zombieIdleState.FindATargetViaLineOfSight(zombieManager);

        if (zombieManager.currentTarget != null)
        {
            currentPhase = RenegatePhase.Chasing;
        }

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
 
    private void HandleReturning(ZombieManager zombieManager)
    {
        if (zombieManager.patrolGraph == null)
            return;

        // Encontrar el nodo más cercano si no tenemos destino de regreso
        if (returnTargetNode == null)
            returnTargetNode = zombieManager.patrolGraph.GetNearestNode(
                zombieManager.transform.position);

        // Navegar hacia el nodo más cercano
        zombieManager.zombieNavmeshAgent.SetDestination(returnTargetNode.transform.position);
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

        // Verificar llegada con NavMesh
        bool arrived = !zombieManager.zombieNavmeshAgent.pathPending
                       && zombieManager.zombieNavmeshAgent.remainingDistance <= returnArrivalThreshold;

        if (arrived)
        {
            // Redetección antes de reanudar patrullaje
            zombieManager.zombieIdleState.FindATargetViaLineOfSight(zombieManager);

            if (zombieManager.currentTarget != null)
            {
                returnTargetNode = null;
                currentPhase = RenegatePhase.Chasing;
                return;
            }

            // Regresar al patrullaje
            zombieManager.zombieIdleState.ResetPatrol();
            returnTargetNode = null;
            currentPhase = RenegatePhase.Chasing;
            return zombieManager.startingState;
        }

        // Redetección durante el regreso
        zombieManager.zombieIdleState.FindATargetViaLineOfSight(zombieManager);
        if (zombieManager.currentTarget != null)
        {
            returnTargetNode = null;
            currentPhase = RenegatePhase.Chasing;
        }
    }
 */