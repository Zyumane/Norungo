using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolGraph : MonoBehaviour
{
    [Header("Nodes in circuit")]
    public List<PatrolNode> patrolNodes = new List<PatrolNode>();

    [ContextMenu("Test Dijkstra")]
    private void TestDijkstra()
    {
        if (patrolNodes.Count < 3)
        {
            Debug.LogWarning("[PatrolGraph] Necesitas al menos 3 nodos para el test.");
            return;
        }
        PatrolNode start = patrolNodes[0];  // Nodo A
        PatrolNode end = patrolNodes[2];    // Nodo C

        List<PatrolNode> path = RunDijkstra(start, end);

        if (path.Count == 0)
        {
            Debug.LogError("[PatrolGraph] Test fallido — ruta vacía.");
            return;
        }

        string result = "[PatrolGraph] Ruta A -> C: ";
        foreach (PatrolNode node in path)
        {
            result += node.name + "->";
        }
        Debug.Log(result);
    }

    [ContextMenu("Diagnostico Grafo")]
    private void DiagnosticoGrafo()
    {
        Debug.Log("[PatrolGraph] Nodos registrados: " + patrolNodes.Count);

        foreach (PatrolNode node in patrolNodes)
        {
            string vecinos = "";
            foreach (PatrolNode neighbor in node.neighbors)
            {
                bool estaEnLista = patrolNodes.Contains(neighbor);
                vecinos += neighbor.name + (estaEnLista ? "(OK)" : "(FALTA EN LISTA)") + " ";
            }
            Debug.Log("[PatrolGraph] " + node.name + " -> vecinos: " + vecinos);
        }
    }

    public List<PatrolNode> RunDijkstra(PatrolNode start,  PatrolNode end)
    {
        // -- Initialization --
        Dictionary<PatrolNode, float> distance = new Dictionary<PatrolNode, float>();
        Dictionary<PatrolNode, PatrolNode> previous = new Dictionary<PatrolNode, PatrolNode>();
        List<PatrolNode> unvisited = new List<PatrolNode>();

        foreach (PatrolNode node in patrolNodes)
        {
            distance[node] = Mathf.Infinity;
            previous[node] = null;
            unvisited.Add(node);
        }

        distance[start] = 0f;

        // -- Main Algorithm -- 
        while (unvisited.Count > 0)
        {
            // Unvisited node with the shortest cumulative distance
            PatrolNode current = null;
            foreach (PatrolNode node in unvisited)
            {
                if (current == null || distance[node] < distance[current])
                {
                    current = node;
                }
            }
            
            unvisited.Remove(current);
            
            if (distance[current] == Mathf.Infinity)
            {
                break;
            }

            //Show edges toward neighbors
            foreach (PatrolNode neighbor in current.neighbors)
            {
                if(!unvisited.Contains(neighbor))
                {
                    continue;
                }

                float edgeWeight = Vector3.Distance(current.transform.position, neighbor.transform.position);
                float newDist = distance[current] + edgeWeight;

                if (newDist < distance[neighbor])
                {
                    distance[neighbor] = newDist;
                    previous[neighbor] = current;
                }
            }
            if(current == end) break;
        }

        // -- Rebuild route --

        List<PatrolNode> path = new List<PatrolNode>();
        PatrolNode step = end;

        while (step != null)
        {
            path.Insert(0, step);
            previous.TryGetValue(step, out step);
        }

        if(path.Count == 0 || path[0] != start)
        {
            Debug.Log("[PatrolGraph] No se encontro ruta de " + start.name + " a " + end.name);
            return new List<PatrolNode>();
        }
        return path;
    }

    public PatrolNode GetNearestNode(Vector3 worldPosition)
    {
        PatrolNode nearest = null;
        float minDist = Mathf.Infinity;

        foreach (PatrolNode node in patrolNodes)
        {
            float dist = Vector3.Distance(worldPosition, node.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = node;
            }
        }
        return nearest;
    }


}
