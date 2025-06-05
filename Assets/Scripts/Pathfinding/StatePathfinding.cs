using System.Collections.Generic;
using UnityEngine;

public class StatePathfinding<T> : StateFollowPoints<T>
{
    protected IMove _move;
    protected Animator _anim;
    protected Transform _target;

    public StatePathfinding(Transform entity, IMove move, Animator anim, Transform target, float distanceToPoint = 0.2f)
        : base(entity, distanceToPoint)
    {
        _move = move;
        _anim = anim;
        _target = target;
    }

    protected override void OnMove(Vector3 dir)
    {
        _move.Move(dir);
        _move.LookDir(dir);
    }

    protected override void OnStartPath()
    {
        _anim.SetFloat("Vel", 1);
    }

    protected override void OnFinishPath()
    {
        _anim.SetFloat("Vel", 0);
    }

    public void SetPathAStarPlusVector()
    {
        Vector3 init = Vector3Int.RoundToInt(_entity.position);
        Vector3 goal = Vector3Int.RoundToInt(_target.position);

        Debug.Log($"[A*] De {init} a {goal}");

        List<Vector3> path = ASTAR.Run<Vector3>(
            init,
            curr => IsSatisfied(curr, goal),
            GetConnections,
            GetCost,
            curr => Heuristic(curr, goal)
        );

        path = ASTAR.CleanPath(path, InView);

        Debug.Log($"[A*] Path generado con {path.Count} puntos");

        SetWaypoints(path);
    }

    private float Heuristic(Vector3 current, Vector3 goal)
    {
        return Vector3.Distance(current, goal);
    }

    private float GetCost(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }

    private bool IsSatisfied(Vector3 curr, Vector3 goal)
    {
        return Vector3.Distance(curr, goal) <= 1.25f && InView(curr, goal);
    }

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
