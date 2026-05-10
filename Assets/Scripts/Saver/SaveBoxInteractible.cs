using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBoxInteractible : InteractibleObject
{
    protected override void Interact(PlayerManager player)
    {
        PlayerInventoryManager inventory = player.playerInventoryManager;
        Item saveKey = FindSaveKey(inventory);

        if (saveKey == null)
        {
            SaveFeedbackUI.Instance.ShowError("Sin llave de guardado");
            return;
        }

        inventory.UseItem(saveKey, player.playerSanityManager);
        GameProgressManager.Instance.SaveFileGame();
        SaveFeedbackUI.Instance.ShowExitu("Partida guardada");

        // STUB — se abre una caja simbolizando el guadado
        Debug.Log("[SAVE BOX] Partida guardada.");
    }

    private Item FindSaveKey(PlayerInventoryManager inventory)
    {
        if (inventory.rightHand != null && inventory.rightHand.GetType() == typeof(SaveKeyItem)) return inventory.rightHand;
        if (inventory.leftHand != null && inventory.leftHand.GetType() == typeof(SaveKeyItem)) return inventory.leftHand;
        if (inventory.beltSlot1 != null && inventory.beltSlot1.GetType() == typeof(SaveKeyItem)) return inventory.beltSlot1;
        if (inventory.beltSlot2 != null && inventory.beltSlot2.GetType() == typeof(SaveKeyItem)) return inventory.beltSlot2;
        if (inventory.beltSlot3 != null && inventory.beltSlot3.GetType() == typeof(SaveKeyItem)) return inventory.beltSlot3;
        return null;
    }
}
