using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : State
{
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

}

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
