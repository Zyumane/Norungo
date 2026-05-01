using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractible : InteractibleObject
{
    [Header("Door propeties")]
    [SerializeField]  Animator doorAnimator;

    [Header("Lock Details")]
    [SerializeField] bool isLocked;
    [SerializeField] bool requiersKey;
    [SerializeField] string keyID;


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    protected override void Interact(PlayerManager player)
    {
        base.Interact(player);

        if(isLocked)
        {
            //Send a UI pop up (This door is locked) 
            //Option 1: if player has key, we auto use the key
            //Option 2: Open Inventory and make the player manually select the key

            //Option 1 logic: scans the key inside of the inventory list
            if(requiersKey && isLocked)
            {
                foreach (Item item in player.playerInventoryManager.itemsInInventory)
                {
                    //if flund the key, the door is unlocked
                    if (item.itemID == keyID)
                    {
                        isLocked = false;
                    }
                }
            }
        }

        if (!isLocked)
        {
            interactibleCanvas.SetActive(false);
            doorAnimator.Play("OpenDoor_Anima");
        }
    }
}
