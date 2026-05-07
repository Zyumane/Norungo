using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    //The state this state character begings on
    public ZombieIdleState startingState;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    public NavMeshAgent zombieNavmeshAgent;

    [Header("Rigidbody")]
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5f;

    [Header("Detection")]
    public float distanceFromCurrentTarget;

    [Header("Current State")]
    //the state this character is currenly on
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;

    [Header("States")]
    public ZombieIdleState zombieIdleState;

    private void Awake()
    {
        currentState = startingState;
        animator = GetComponentInChildren<Animator>();
        zombieNavmeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(currentTarget != null)
        {
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        State nextState;
        //Run logic, based on which state we are currenly in
        if(currentState != null)
        {
            nextState = currentState.Tick(this);
            
            if (nextState != null)
            {
                currentState = nextState;
            }
        }
        //If Logic is met to swich to the next state, we change states
    }


}
