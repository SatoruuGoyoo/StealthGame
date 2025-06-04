using System.Collections.Generic;
using UnityEngine;

public class WeightedPatrolPointManager : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool showGizmos = true;

    [Header("Generated Patrol Points")]
    [SerializeField] private List<Vector2Int> patrolPoints = new List<Vector2Int>();
    [SerializeField] private List<float> weights = new List<float>();

    private RouletteWheel<WeightedPatrolPoint> _roulette = new RouletteWheel<WeightedPatrolPoint>();
    private List<WeightedPatrolPoint> _weightedPoints = new List<WeightedPatrolPoint>();

    private void Awake()
    {
        BuildRoulette();
    }

    private void BuildRoulette()
    {
        _roulette.Clear();
        _weightedPoints.Clear();

        for (int i = 0; i < Mathf.Min(patrolPoints.Count, weights.Count); i++)
        {
            var wp = new WeightedPatrolPoint(patrolPoints[i], weights[i]);
            _weightedPoints.Add(wp);
            _roulette.AddItem(wp, weights[i]);
        }
    }

    public Vector2Int GetRandomPoint()
    {
        var p = _roulette.GetRandom();
        return p != null ? p.GridPosition : Vector2Int.zero;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || _weightedPoints == null) return;

        float maxWeight = 0.01f;
        foreach (var wp in _weightedPoints)
            if (wp.Weight > maxWeight) maxWeight = wp.Weight;

        foreach (var wp in _weightedPoints)
        {
            Vector3 world = new Vector3(wp.GridPosition.x, 0f, wp.GridPosition.y);
            float norm = wp.Weight / maxWeight;
            Gizmos.color = Color.Lerp(Color.gray, Color.red, norm);
            Gizmos.DrawSphere(world, 0.3f);
        }
    }
}
