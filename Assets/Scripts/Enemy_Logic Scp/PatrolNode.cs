using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
    [Header("Neighbors")]
    public List<PatrolNode> neighbors = new List<PatrolNode>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.color = Color.magenta;
        foreach (PatrolNode neigbor in neighbors)
        {
            if(neigbor != null)
            {
                Gizmos.DrawLine(transform.position, neigbor.transform.position);
            }
        }
    }



}
