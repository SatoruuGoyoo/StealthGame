using System.Collections.Generic;
using UnityEngine;

public static class SearchHelper
{
    public static List<Vector3> GetWeightedSearchPoints(Vector3 origin, int count, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        List<float> weights = new List<float>();

        for (int i = 0; i < count * 2; i++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            Vector3 candidate = origin + new Vector3(dir.x, 0, dir.y) * Random.Range(radius * 0.5f, radius);

            if (!ObstacleManager.Instance.IsRightPos(candidate))
                continue;

            float weight = 1f - Vector3.Distance(origin, candidate) / radius;
            points.Add(candidate);
            weights.Add(weight);
        }

        return RouletteSelection(points, weights, count);
    }

    private static List<Vector3> RouletteSelection(List<Vector3> candidates, List<float> weights, int count)
    {
        List<Vector3> selected = new List<Vector3>();
        float total = 0f;
        foreach (var w in weights) total += w;

        for (int i = 0; i < count && candidates.Count > 0; i++)
        {
            float r = Random.Range(0f, total);
            float sum = 0f;
            for (int j = 0; j < candidates.Count; j++)
            {
                sum += weights[j];
                if (r <= sum)
                {
                    selected.Add(candidates[j]);
                    total -= weights[j];
                    candidates.RemoveAt(j);
                    weights.RemoveAt(j);
                    break;
                }
            }
        }

        return selected;
    }
}
