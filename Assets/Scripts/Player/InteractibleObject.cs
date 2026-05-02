using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour
{
    //Clase base para cualquier objeto interactivo (consumibles, puertas, palancas, Etc.)

    protected PlayerManager player; //El jugador interacciona con el objeto
    protected Collider interactibleCollider; //el colisionador que activa la imagen dentro del objeto

    [SerializeField] protected GameObject interactibleCanvas; //La imagen que indica al jugador que puede interactuar con el objeto

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Opcional: revisa la capa o layer especifica de la colision

        if(player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if(player  != null)
        {
            interactibleCanvas.SetActive(true);
            player.canInteract = true;
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if(player != null)
        {
            if(player.inputManager.interactInput)
            {
                Interact(player);
                player.inputManager.interactInput = false;
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            interactibleCanvas.SetActive(false);
            player.canInteract = false;
            player = null;
        }
    }

    protected virtual void Interact(PlayerManager player)
    {
        Debug.Log("Interacted the object");
    }



}
