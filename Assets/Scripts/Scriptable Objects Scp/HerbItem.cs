using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/HerbItem")]
public class HerbItem : Item
{
    [Header("Herb Settings")]
    public int sanityRestoreAmount = 20;

    public void Consume(PlayerSanityManager sanityManager)
    {
        sanityManager.RestoreSanity(sanityRestoreAmount);
        Debug.Log($"Hierba consumida — cordura restaurada: + {sanityRestoreAmount}");
    }
}
