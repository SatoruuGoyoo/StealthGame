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

                if (ObstacleManager.Instance.IsRightPos(candidate))
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
}
