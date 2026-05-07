using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    //This is the base class for all future states
   
    public virtual State Tick(ZombieManager zombieManager)
    {

        Debug.Log("Running State");
        return this;
    }


}
/*code for testing -- scrapped code
    public bool moveToChaseTargetState;
    public bool moveToSleepState;
    
    if(moveToChaseTargetState)
        {
            Debug.Log("Swiching to chase target state");
            return this;
            //Return different state --> Which changes the state to the new state
        }
        else if(moveToSleepState)
        {
            Debug.Log("Swiching to sleep state");
            return this;
        }
 
 */