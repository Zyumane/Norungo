using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInteractible : InteractibleObject
{
    [SerializeField] Item item;

    private PlayerInventoryManager inventory;

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
        inventory = player.playerInventoryManager;

        //Check for space
        //Select which slot on grid item will be placed
        //player.playerInventoryManager.itemsInInventory.Add(item);
        //gameObject.SetActive(false);

        // Intenta asignar a mano derecha
        if (inventory.rightHand == null)
        {
            inventory.rightHand = item;
            gameObject.SetActive(false);
            return;
        }
        // mano izquierda
        if (inventory.leftHand == null)
        {
            inventory.leftHand = item;
            gameObject.SetActive(false);
            return;
        }
        if(inventory.AddToBelt(item))
        {
            gameObject.SetActive(false);
            return;
        }
        Debug.Log("Inventario lleno — no se puede recoger el objeto.");
    }

}
