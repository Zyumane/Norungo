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

    public bool BeltAvailable => gotBelt;

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
    }


}
