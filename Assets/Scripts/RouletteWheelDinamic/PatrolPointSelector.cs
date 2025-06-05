using System.Collections.Generic;
using UnityEngine;

public class PatrolPointSelector : MonoBehaviour
{
    public List<WeightedPatrolPoint> patrolPoints = new();

    public Transform GetNextPatrolPoint()
    {
        float totalWeight = 0f;
        foreach (var p in patrolPoints)
            totalWeight += p.weight;

        float rand = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var p in patrolPoints)
        {
            cumulative += p.weight;
            if (rand <= cumulative)
                return p.point;
        }

        return patrolPoints.Count > 0 ? patrolPoints[0].point : null;
    }

    public void UpdateWeights(Vector3 npcPos)
    {
        foreach (var p in patrolPoints)
        {
            float dist = Vector3.Distance(npcPos, p.point.position);
            p.weight = Mathf.Max(0.1f, 1f / dist);
        }
    }
    private void OnDrawGizmos()
    {
        if (patrolPoints == null) return;
        Gizmos.color = Color.yellow;
        foreach (var p in patrolPoints)
        {
            if (p.point != null)
                Gizmos.DrawWireSphere(p.point.position, 0.3f);
        }
    }
}
