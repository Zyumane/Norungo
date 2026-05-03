using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSanityManager : MonoBehaviour
{
    PlayerManager player;

    [Header("Sanity Flags")]
    public int playerSanity = 100;
    public int maxSanity = 100;

    [Header("Damage flags")]
    public int sanityDamageRate = 5;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void TakeSanityDamage()
    {
        playerSanity -= sanityDamageRate;
        playerSanity = Mathf.Clamp(playerSanity, 0, maxSanity);

        if(playerSanity <= 0)
        {
            OnSanityDepleted();
        }
    }

    public void RestoreSanity(int amount)
    {
        playerSanity += amount;
        playerSanity = Mathf.Clamp(playerSanity, 0, maxSanity);
    }

    private void OnSanityDepleted()
    {
        Debug.Log("Cordura agotada — estado crítico activo.");
    }
}
