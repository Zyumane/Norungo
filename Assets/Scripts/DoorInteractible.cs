using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractible : InteractibleObject
{
    [Header("Door propeties")]
    [SerializeField]  Animator doorAnimator;

    private PlayerInventoryManager inventory;

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
        //base.Interact(player);

        if(isLocked)
        {
            if(requiersKey)
            {
                inventory = player.playerInventoryManager;

                Item foundKey = FindKey(inventory);

                if(foundKey != null)
                {
                    isLocked = false;

                    if(inventory.rightHand == foundKey)
                    {
                        inventory.rightHand = null;
                    }
                    else if(inventory.leftHand == foundKey)
                    {
                        inventory.leftHand = null;
                    }
                    else if(inventory.beltSlot1 == foundKey)
                    {
                        inventory.beltSlot1 = null;
                    }
                    else if(inventory.beltSlot2 == foundKey)
                    {
                        inventory.beltSlot2 = null;
                    }
                    else if(inventory.beltSlot3  == foundKey)
                    {
                        inventory.beltSlot3 = null;
                    }
                }
                else
                {
                    Debug.Log("The door requiers key, or it has not been assigned");
                    return;
                }
            }
            else
            {
                Debug.Log("The Door is locked");
                return;
            }
        }

        if (!isLocked)
        {
            interactibleCanvas.SetActive(false);
            doorAnimator.Play("OpenDoor_Anima");
        }
    }

    private Item FindKey(PlayerInventoryManager inventory)
    {
        if(string.IsNullOrEmpty(keyID))
        {
            if (inventory.rightHand != null && inventory.rightHand.GetType() == typeof(KeyItem))
                return inventory.rightHand;
            if (inventory.leftHand != null && inventory.leftHand.GetType() == typeof(KeyItem))
                return inventory.leftHand;
            if (inventory.beltSlot1 != null && inventory.beltSlot1.GetType() == typeof(KeyItem))
                return inventory.beltSlot1;
            if (inventory.beltSlot2 != null && inventory.beltSlot2.GetType() == typeof(KeyItem))
                return inventory.beltSlot2;
            if (inventory.beltSlot3 != null && inventory.beltSlot3.GetType() == typeof(KeyItem))
                return inventory.beltSlot3;
            return null;
        }

        if (inventory.rightHand != null && inventory.rightHand.itemID == keyID)
            return inventory.rightHand;
        if (inventory.leftHand != null && inventory.leftHand.itemID == keyID) 
            return inventory.leftHand;
        if (inventory.beltSlot1 != null && inventory.beltSlot1.itemID == keyID)
            return inventory.beltSlot1;
        if (inventory.beltSlot2 != null && inventory.beltSlot2.itemID == keyID)
            return inventory.beltSlot2;
        if (inventory.beltSlot3 != null && inventory.beltSlot3.itemID == keyID)
            return inventory.beltSlot3;
        return null;
    }

}

/*   Scrapped Code
 
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
 
        if(inventory.rightHand != null && inventory.rightHand.itemID == keyID)
            return inventory.rightHand;
        if(inventory.leftHand != null && inventory.leftHand.itemID == keyID)
            return inventory.leftHand;
        if(inventory.beltSlot1 != null && inventory.beltSlot1.itemID == keyID)
            return inventory.beltSlot1;
        if(inventory.beltSlot2 != null && inventory.beltSlot2.itemID == keyID)
            return inventory.beltSlot2;
        if(inventory.beltSlot3 != null && inventory.beltSlot3.itemID == keyID)
            return inventory.beltSlot3;

        return null;    
 
 */
