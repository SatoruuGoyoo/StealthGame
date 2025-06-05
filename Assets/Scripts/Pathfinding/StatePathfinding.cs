using System.Collections.Generic;
using UnityEngine;

public class StatePathfinding<T> : StateFollowPoints<T>
{
    protected IMove _move;
    private Animator _anim;

    public StatePathfinding(Transform entity, IMove move, Animator anim, float distanceToPoint = 0.2f)
        : base(entity, distanceToPoint)
    {
        _move = move;
        _anim = anim;
    }

    protected override void OnMove(Vector3 dir)
    {
        _move.Move(dir);
        _move.LookDir(dir);
    }

    protected override void OnStartPath() => _anim.SetFloat("Vel", 1);
    protected override void OnFinishPath() => _anim.SetFloat("Vel", 0);

    public void SetPathTo(Vector3 goal)
    {
        Vector3 init = Vector3Int.RoundToInt(_entity.position);
        goal = Vector3Int.RoundToInt(goal);

        List<Vector3> path = ASTAR.Run<Vector3>(
            init,
            curr => Vector3.Distance(curr, goal) <= 2f && InView(curr, goal),
            GetConnections,
            GetCost,
            curr => Vector3.Distance(curr, goal)
        );

        if (path == null || path.Count < 2)
        {
            Debug.LogWarning($"[CHASE] Ignorado path: count = {path?.Count ?? 0}");
            return;
        }
        else
        {
            Debug.Log($"[CHASE] Path con {path.Count} puntos");
        }


        path = ASTAR.CleanPath(path, InView);
        SetWaypoints(path);

        for (int i = 0; i < path.Count - 1; i++)
            Debug.DrawLine(path[i] + Vector3.up * 0.2f, path[i + 1] + Vector3.up * 0.2f, Color.cyan, 1.5f);
    }

    // Métodos auxiliares (idénticos)
    private float GetCost(Vector3 from, Vector3 to) => Vector3.Distance(from, to);
    private List<Vector3> GetConnections(Vector3 curr)
    {
        var neighbors = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                var neighbor = curr + new Vector3(x, 0, z);
                if (ObstacleManager.Instance.IsRightPos(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
        }
        return neighbors;
    }

    private bool InView(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        return !Physics.Raycast(from, dir.normalized, dir.magnitude, PathfindingConstants.obsMask);
    }

}
