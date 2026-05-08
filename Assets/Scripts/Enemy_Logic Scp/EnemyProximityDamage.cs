using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityDamage : MonoBehaviour
{
    [Header("Proximity Flags")]
    public float detectionRadius = 12f;
    public float damageRadius = 5f;
    public float gameOverRadius = 1.5f;

    [Header("Damage Settings")]
    public int damageInterval = 1;
    private float damageTimer = 0f;

    private void Update()
    {
        damageTimer += Time.deltaTime;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.layer != LayerMask.NameToLayer("Player"))
                continue;

            PlayerManager player = hit.GetComponent<PlayerManager>();

            if (player == null)
                continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance <= gameOverRadius)
            {
                if (GameOverManager.Instance != null)
                    GameOverManager.Instance.TriggerGameOver();
                return;
            }


            // Esfera 2 — Daño de cordura
            if (distance <= damageRadius)
            {
                if (damageTimer >= damageInterval)
                {
                    player.playerSanityManager.TakeSanityDamage();
                    Debug.Log("[ESFERA 2] Daño de cordura. Cordura actual: " + player.playerSanityManager.playerSanity);
                    damageTimer = 0f;
                }
                return;
            }

            // Esfera 1 — Detección
            if (distance <= detectionRadius)
            {
                if (damageTimer >= damageInterval)
                {
                    Debug.Log("[ESFERA 1] Jugador detectado. Distancia: " + distance.ToString("F1"));
                    damageTimer = 0f;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere (transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere (transform.position, damageRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere (transform.position, gameOverRadius);
    }
}

/* Scrapped code
 
    if (distance <= gameOverRadius)
            {
                if (damageTimer >= damageInterval)
                {
                    Debug.Log("[ESFERA 3] Game Over por proximidad extrema.");
                    damageTimer = 0f;
                }
                return;
            }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, other.transform.position);

        //Sphere 3 - Game over 
        if(distance <= damageRadius)
        {
            if(damageTimer >= damageInterval)
            {
                player.playerSanityManager.TakeSanityDamage();
                damageTimer = 0;
            }
        }
    }




*/