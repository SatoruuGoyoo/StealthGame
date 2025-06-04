using UnityEngine;

public class WeightedPatrolPoint
{
    public Vector2Int GridPosition;
    public float Weight;

    public WeightedPatrolPoint(Vector2Int gridPos, float weight)
    {
        GridPosition = gridPos;
        Weight = weight;
    }
}
