using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInteractible : InteractibleObject
{
    [SerializeField] Item item;
    //[SerializeField]



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

        //Check for space

        //Select which slot on grid item will be placed

        player.playerInventoryManager.itemsInInventory.Add(item);
        gameObject.SetActive(false);
    }

}
