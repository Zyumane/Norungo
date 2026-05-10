using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [Header("Belt state")]
    public bool gotBelt = false;

    [Header("Manos fase 1 y 2")]
    public Item rightHand;
    public Item leftHand;

    [Header("Belt contents")]
    public Item beltSlot1;  
    public Item beltSlot2;
    public Item beltSlot3;

    [Header("Slot Navigation")]
    public int activeSlotIndex = 0;
    public int lastHandIndex = 0;

    public bool BeltAvailable => gotBelt;

    public void SetActiveSlot(int index)
    {
        activeSlotIndex = index;

        // Actualiza lastHandIndex solo cuando el slot activo es una mano
        if (index == 0 || index == 1)
            lastHandIndex = index;
        Debug.Log("Slot activo: " + index);
    }

    public bool hasFreeBeltSlot()
    {
        if(!gotBelt)
            return false;
        return beltSlot1 == null || beltSlot2 == null || beltSlot3 == null;
    }

    public bool AddToBelt(Item item)
    {
        if(!gotBelt)
            return false;

        if(beltSlot1 == null)
        {
            beltSlot1 = item;
            return true;
        }
        if(beltSlot2 == null)
        {
            beltSlot2 = item;
            return true;
        }
        if(beltSlot3 == null)
        {
            beltSlot3 = item;
            return true;
        }
        return false;
    }

    public void UnlockBelt()
    {
        gotBelt = true;
        Debug.Log("Cinturón desbloqueado — 3 slots adicionales disponibles.");
        FindObjectOfType<Inventory_HUD_UI>().PlayUnlockBeltAnimation();
    }

    public void UseItem(Item item, PlayerSanityManager sanityManager)
    {
        if (item == null) return;

        if (item.GetType() == typeof(HerbItem))
        {
            HerbItem herb = (HerbItem)item;
            herb.Consume(sanityManager);
            RemoveItem(item);
        }
        else if (item.GetType() == typeof(SaveKeyItem))
        {
            RemoveItem(item);
        }
    }

    private void RemoveItem(Item item)
    {
        if (rightHand == item) { rightHand = null; return; }
        if (leftHand == item) { leftHand = null; return; }
        if (beltSlot1 == item) { beltSlot1 = null; return; }
        if (beltSlot2 == item) { beltSlot2 = null; return; }
        if (beltSlot3 == item) { beltSlot3 = null; return; }
    }

    public Item GetActiveItem()
    {
        switch(activeSlotIndex)
        {
            case 0: return rightHand;
            case 1: return leftHand;
            case 2: return beltSlot1;
            case 3: return beltSlot2;
            case 4: return beltSlot3;
            default: return null;
        }
    }


}
