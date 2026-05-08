using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : State
{
    ZombiePursuitState pursuitTargetState;

    [Header("Detection Layer")]
    [SerializeField] LayerMask detectionLayer;

    [Header("Line Of Sight Detection")]
    [SerializeField] float characterEyeLevel = 1.8f;
    [SerializeField] LayerMask ignoreForLineOfSightDetection;

    [Header("Detection Radius")]
    [SerializeField] float detectionRadius = 5f;

    [Header("Detection Angle Radius")]
    [SerializeField] float minimumDetectionRadiusAngle = -50f;
    [SerializeField] float maximumDetectionRadiusAngle = 50f;

    [Header("Patrol")]
    [SerializeField] private float arrivalThreshold = 1.0f;

    private List<PatrolNode> currentPath = new List<PatrolNode>();
    private int currentPathIndex = 0;

    private void Awake()
    {
        pursuitTargetState = GetComponent<ZombiePursuitState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        // Si la fase no es Patrolling, ceder control a ZombiePursuitState
        if (zombieManager.currentPhase != ZombieManager.RenegatePhase.Patrolling
            && zombieManager.currentPhase != ZombieManager.RenegatePhase.Chasing)
            return pursuitTargetState;

        if (zombieManager.currentTarget != null)
        {
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
            return pursuitTargetState;
        }

        FindATargetViaLineOfSight(zombieManager);

        if (zombieManager.currentTarget != null)
        {
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing;
            return pursuitTargetState;
        }

        HandlePatrol(zombieManager);
        return this;
    }

    private void HandlePatrol(ZombieManager zombieManager)
    {
        if (zombieManager.currentPhase == ZombieManager.RenegatePhase.Chasing)
            return;

        zombieManager.currentPhase = ZombieManager.RenegatePhase.Patrolling;

        if (zombieManager.patrolGraph == null)
            return;

        if (currentPath == null || currentPath.Count == 0)
        {
            PatrolNode startNode = zombieManager.patrolGraph.GetNearestNode(
                zombieManager.transform.position);

            if (startNode == null)
                return;

            PatrolNode nextNode = GetNextNodeInCycle(zombieManager, startNode);
            currentPath = zombieManager.patrolGraph.RunDijkstra(startNode, nextNode);
            currentPathIndex = 0;

            if (currentPath.Count == 0)
                return;
        }

        PatrolNode target = currentPath[currentPathIndex];
        zombieManager.zombieNavmeshAgent.SetDestination(target.transform.position);
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

        bool arrived = !zombieManager.zombieNavmeshAgent.pathPending
                       && zombieManager.zombieNavmeshAgent.remainingDistance <= arrivalThreshold;

        if (arrived)
        {
            currentPathIndex++;

            if (currentPathIndex >= currentPath.Count)
            {
                PatrolNode lastNode = currentPath[currentPath.Count - 1];
                PatrolNode nextNode = GetNextNodeInCycle(zombieManager, lastNode);
                currentPath = zombieManager.patrolGraph.RunDijkstra(lastNode, nextNode);
                currentPathIndex = 1;
            }
        }
    }

    private PatrolNode GetNextNodeInCycle(ZombieManager zombieManager, PatrolNode current)
    {
        List<PatrolNode> nodes = zombieManager.patrolGraph.patrolNodes;
        int index = nodes.IndexOf(current);
        int nextIndex = (index + 1) % nodes.Count;
        return nodes[nextIndex];
    }

    public void ResetPatrol()
    {
        currentPath = new List<PatrolNode>();
        currentPathIndex = 0;
    }

    public void FindATargetViaLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionRadiusAngle &&
                    viewableAngle < maximumDetectionRadiusAngle)
                {
                    RaycastHit hit;
                    Vector3 playerPoint = new Vector3(
                        player.transform.position.x, characterEyeLevel,
                        player.transform.position.z);
                    Vector3 zombiePoint = new Vector3(
                        transform.position.x, characterEyeLevel,
                        transform.position.z);

                    Debug.DrawLine(playerPoint, zombiePoint, Color.yellow);

                    if (!Physics.Linecast(playerPoint, zombiePoint,
                        out hit, ignoreForLineOfSightDetection))
                    {
                        zombieManager.currentTarget = player;
                    }
                }
            }
        }
    }

    public bool IsPlayerInLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionRadiusAngle &&
                    viewableAngle < maximumDetectionRadiusAngle)
                {
                    Vector3 playerPoint = new Vector3(
                        player.transform.position.x, characterEyeLevel,
                        player.transform.position.z);
                    Vector3 zombiePoint = new Vector3(
                        transform.position.x, characterEyeLevel,
                        transform.position.z);

                    if (!Physics.Linecast(playerPoint, zombiePoint,
                        ignoreForLineOfSightDetection))
                        return true;
                }
            }
        }

        return false;
    }
}

/*Major Manual Rollback
 //We make our character idle until they find a potential target
    //If a target is found we proceed to the "pursuitTarget" state
    //If no target is found we remain in the idle state position
    ZombiePursuitState pursuitTargetState;

    [Header("Detection Layer")]
    [SerializeField] LayerMask detectionLayer;

    [Header("Line Of Sight Detection")]
    [SerializeField] float characterEyeLevel = 1.8f;
    [SerializeField] LayerMask ignoreForLineOfSightDetection;

    [Header("Detection Radius")]
    [SerializeField] float detectionRadius = 5f;

    [Header("Detection Angle Radius")]
    [SerializeField] float minimumDetectionRadiusAngle = -50f;
    [SerializeField] float maximumDetectionRadiusAngle = 50f;

    [Header("Patrol")]
    [SerializeField] private float arrivalThreshold = 1.0f;

    private List<PatrolNode> currentPath = new List<PatrolNode>();
    private int currentPathIndex = 0;

    private void Awake()
    {
        pursuitTargetState = GetComponent<ZombiePursuitState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing; // <- AGREGAR
            return pursuitTargetState;
        }

        FindATargetViaLineOfSight(zombieManager);

        if (zombieManager.currentTarget != null)
        {
            zombieManager.currentPhase = ZombieManager.RenegatePhase.Chasing; // <- AGREGAR
            return pursuitTargetState;
        }

        HandlePatrol(zombieManager);
        return this;
    }

    private void HandlePatrol(ZombieManager zombieManager)
    {
        // Solo patrullar si realmente estamos en idle
        if (zombieManager.currentPhase == ZombieManager.RenegatePhase.Chasing)
            return;

        // ... resto del método sin cambios
        zombieManager.currentPhase = ZombieManager.RenegatePhase.Patrolling;
        if (zombieManager.patrolGraph == null)
            return;

        // Si no hay ruta calculada, calcular desde el nodo más cercano
        if (currentPath == null || currentPath.Count == 0)
        {
            PatrolNode startNode = zombieManager.patrolGraph.GetNearestNode(
                zombieManager.transform.position);

            if (startNode == null)
                return;

            // Siguiente nodo en el ciclo
            PatrolNode nextNode = GetNextNodeInCycle(zombieManager, startNode);

            currentPath = zombieManager.patrolGraph.RunDijkstra(startNode, nextNode);
            currentPathIndex = 0;

            if (currentPath.Count == 0)
                return;
        }

        // Navegar hacia el nodo actual del path
        PatrolNode target = currentPath[currentPathIndex];
        zombieManager.zombieNavmeshAgent.SetDestination(target.transform.position);
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

        float distanceToTarget = Vector3.Distance(zombieManager.transform.position, target.transform.position);

        bool arrived = !zombieManager.zombieNavmeshAgent.pathPending && zombieManager.zombieNavmeshAgent.remainingDistance <= arrivalThreshold;

        if (arrived) //from: distanceToTarget <= arrivalThreshold
        {
            currentPathIndex++;

            // Llegamos al final del path — calcular siguiente tramo del ciclo
            if (currentPathIndex >= currentPath.Count)
            {
                PatrolNode lastNode = currentPath[currentPath.Count - 1];
                PatrolNode nextNode = GetNextNodeInCycle(zombieManager, lastNode);

                currentPath = zombieManager.patrolGraph.RunDijkstra(lastNode, nextNode);
                currentPathIndex = 0;
            }
        }
    }

    // Devuelve el siguiente nodo en el ciclo hamiltoniano
    private PatrolNode GetNextNodeInCycle(ZombieManager zombieManager, PatrolNode current)
    {
        List<PatrolNode> nodes = zombieManager.patrolGraph.patrolNodes;
        int index = nodes.IndexOf(current);
        int nextIndex = (index + 1) % nodes.Count;
        return nodes[nextIndex];
    }

    // Resetea el patrullaje — llamado desde HandleReturning()
    public void ResetPatrol()
    {
        currentPath = new List<PatrolNode>();
        currentPathIndex = 0;
    }

    public void FindATargetViaLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionRadiusAngle &&
                    viewableAngle < maximumDetectionRadiusAngle)
                {
                    RaycastHit hit;
                    Vector3 playerStartPoint = new Vector3(
                        player.transform.position.x, characterEyeLevel,
                        player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(
                        transform.position.x, characterEyeLevel,
                        transform.position.z);

                    Debug.DrawLine(playerStartPoint, zombieStartPoint, Color.yellow);

                    if (Physics.Linecast(playerStartPoint, zombieStartPoint,
                        out hit, ignoreForLineOfSightDetection))
                    {
                        Debug.Log("There is something in the way");
                    }
                    else
                    {
                        Debug.Log("We have a target, switching states");
                        zombieManager.currentTarget = player;
                    }
                }
            }
        }
    }

    public bool IsPlayerInLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionRadiusAngle &&
                    viewableAngle < maximumDetectionRadiusAngle)
                {
                    Vector3 playerStartPoint = new Vector3(
                        player.transform.position.x, characterEyeLevel,
                        player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(
                        transform.position.x, characterEyeLevel,
                        transform.position.z);

                    if (!Physics.Linecast(playerStartPoint, zombieStartPoint,
                        ignoreForLineOfSightDetection))
                        return true;
                }
            }
        }
        return false;
    }
 */

/*private void FindATargetViaLineOfSight(ZombieManager zombieManager)
   {
       //We are searching all colliders on the layer of the player within a certian radious
       Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

       Debug.Log("We are checking for colliders");

       //For every collider that we find, that is on the same layer of the player,
       //we try and search it for a PlayerManager script
       for(int i = 0; i < colliders.Length; i++)
       {
           PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

           //if the player manager is detected then we then chack for aline of sight
           if(player != null)
           {
               Debug.Log("We have found the player collider");

               //the target must be in front of us
               Vector3 targetDirection = transform.position - player.transform.position;
               float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

               if (viewableAngle > minimumDetectionRadiusAngle && viewableAngle < maximumDetectionRadiusAngle)
               {
                   RaycastHit hit;
                   Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterEyeLevel, player.transform.position.z);
                   Vector3 zombieStartPoint = new Vector3(transform.position.x, characterEyeLevel, transform.position.z);

                   Debug.DrawLine(playerStartPoint, zombieStartPoint, Color.red);

                   //Check one last time for object blocking view
                   if(Physics.Linecast(playerStartPoint, zombieStartPoint, out hit))
                   {
                       Debug.Log("There is something in the way");
                       //Cannot find the target, there is an object in the way
                   }
                   else
                   {
                       Debug.Log("We have a target, switching states");
                       zombieManager.currentTarget = player;
                   }
               }
           }
       }
   }*/

/*Manual RollBack
    if (zombieManager.currentTarget != null)
            return pursuitTargetState;

        FindATargetViaLineOfSight(zombieManager);

        if (zombieManager.currentTarget != null)
            return pursuitTargetState;

        HandlePatrol(zombieManager);
        return this;

    ZombiePursuitState pursuitTargetState;

    [Header("Detection Layer")]
    [SerializeField] LayerMask detectionLayer;

    [Header("Line Of Sight Detection")]
    [SerializeField] float characterEyeLevel = 1.8f;
    [SerializeField] LayerMask ignoreForLineOfSightDetection;

    [Header("Detection Radius")]
    [SerializeField] float detectionRadius = 5f;

    [Header("Detection Angle Radius")]
    [SerializeField] float minimumDetectionRadiusAngle = -50f;
    [SerializeField] float maximumDetectionRadiusAngle = 50f;


    private void Awake()
    {
        pursuitTargetState = GetComponent<ZombiePursuitState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        //Logic to find a target goes here
        if(zombieManager.currentTarget != null) 
        {
            //Debug.Log("We have found a target");
            return pursuitTargetState;
        }
        else
        {
            //Debug.Log("We have no target yet");
            FindATargetViaLineOfSight(zombieManager);
            return this;
        }
    }
    
    public void FindATargetViaLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                // Dirección corregida: del zombie al jugador
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionRadiusAngle && viewableAngle < maximumDetectionRadiusAngle)
                {
                    RaycastHit hit;
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterEyeLevel, player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(transform.position.x, characterEyeLevel, transform.position.z);

                    Debug.DrawLine(playerStartPoint, zombieStartPoint, Color.yellow);

                    if (Physics.Linecast(playerStartPoint, zombieStartPoint, out hit, ignoreForLineOfSightDetection))
                    {
                        Debug.Log("There is something in the way");
                    }
                    else
                    {
                        Debug.Log("We have a target, switching states");
                        zombieManager.currentTarget = player;
                    }
                }
            }
        }
    }

    public bool IsPlayerInLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionRadiusAngle && viewableAngle < maximumDetectionRadiusAngle)
                {
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterEyeLevel, player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(transform.position.x, characterEyeLevel, transform.position.z);

                    if (!Physics.Linecast(playerStartPoint, zombieStartPoint, ignoreForLineOfSightDetection))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
 */
