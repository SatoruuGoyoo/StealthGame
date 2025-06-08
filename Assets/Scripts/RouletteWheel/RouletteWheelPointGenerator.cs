using System.Collections.Generic;
using UnityEngine;

public static class RouletteWheelPointGenerator
{
    public static List<Vector3> GeneratePoints(Vector3 center, int count, float radius = 4f)
    {
        List<Vector3> points = new List<Vector3>();
        int maxTriesPerPoint = 10;

        for (int i = 0; i < count; i++)
        {
            bool foundValid = false;

            for (int attempt = 0; attempt < maxTriesPerPoint; attempt++)
            {
                Vector2 rand = Random.insideUnitCircle.normalized * radius;
                Vector3 candidate = center + new Vector3(rand.x, 0, rand.y);
                candidate = Vector3Int.RoundToInt(candidate);

                if (IsPointClear(candidate, 2f) && !IsTooCloseToOthers(candidate, points, 3.5f))
                {
                    points.Add(candidate);
                    foundValid = true;
                    break;
                }
            }
            if (!foundValid)
            {
                points.Add(Vector3Int.RoundToInt(center));
            }
        }
        return points;
    }

    private static bool IsPointClear(Vector3 point, float clearanceRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(point, clearanceRadius, PathfindingConstants.obsMask);
        return colliders.Length == 0 && ObstacleManager.Instance.IsRightPos(point);
    }

    private static bool IsTooCloseToOthers(Vector3 point, List<Vector3> others, float minDistance)
    {
        foreach (var p in others)
        {
            if (Vector3.Distance(p, point) < minDistance)
                return true;
        }
        return false;
    }
}
