using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    PlayerManager player;
    SanityStatusUI sanityStatusUI;


    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        sanityStatusUI = FindObjectOfType<SanityStatusUI>();
    }

    public void DisplaySanityStatus()
    {
        if(sanityStatusUI == null)
            return;
        sanityStatusUI.DisplaySanityStatus(player.playerSanityManager.playerSanity);
    }

    public void ToggleSanityHUD()
    {
        if(sanityStatusUI == null)
        {
            return;
        }
        bool current = !sanityStatusUI.isInPersistentMode;
        sanityStatusUI.SetPersistentMode(current);

        if (current)
        {
            DisplaySanityStatus();
        }
    }

    public void ResetSanityToggle()
    {
        sanityStatusUI.SetPersistentMode(false);
    }
}
