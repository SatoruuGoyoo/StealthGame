using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Conections")]
    public List<Node> neightbourds = new List<Node>();

    [Header("Using Trap?")]
    public bool hasTrap = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = hasTrap ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);

        // Líneas a vecinos (debug visual)
        Gizmos.color = Color.cyan;
        foreach (var neighbor in neightbourds)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}
