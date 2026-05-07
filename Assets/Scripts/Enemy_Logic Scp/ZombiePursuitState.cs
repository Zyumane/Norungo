using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuitState : State
{

    public override State Tick(ZombieManager zombieManager)
    {
        MoveTorwardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);
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