using System.Collections.Generic;
using UnityEngine;

public class StatePathfinding<T> : StateFollowPoints<T>
{
    protected IMove _move;
    protected Animator _anim;
    protected ObstacleAvoidance _avoidance;

    public StatePathfinding(Transform entity, IMove move, Animator anim, float distanceToPoint = 0.2f)
        : base(entity, distanceToPoint)
    {
        _move = move;
        _anim = anim;
    }

    public override void Initialize(params object[] p)
    {
        base.Initialize(p);
        _move = p[0] as IMove;
        var look = p[1] as ILook;
        var model = p[2] as NPCModel;

        _anim = (model as MonoBehaviour).GetComponent<Animator>();
        _avoidance = model.GetComponent<ObstacleAvoidance>();
    }


    protected override void OnMove(Vector3 dir)
    {
        if (_avoidance != null)
            dir = _avoidance.GetDir(dir);

        _move.Move(dir.normalized);
        _move.LookDir(dir.normalized);
    }

    protected override void OnStartPath() => _anim.SetFloat("Vel", 1);
    protected override void OnFinishPath() => _anim.SetFloat("Vel", 0);

    public void SetPathAStarPlusVector(Vector3 init, Vector3 goal)
    {
        if (!ObstacleManager.Instance.IsRightPos(goal))
        {
            Debug.LogWarning($"Goal inválido para pathfinding: {goal}");
            return;
        }

        List<Vector3> path = ASTAR.Run<Vector3>(
            init,
            curr => IsSatisfied(curr, goal),
            GetConnections,
            GetCost,
            curr => Heuristic(curr, goal)
        );

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("Path vacío");
            return;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i] + Vector3.up * 0.2f, path[i + 1] + Vector3.up * 0.2f, Color.red, 1f);
        }

        path = ASTAR.CleanPath(path, InView);
        SetWaypoints(path);
    }

    // ======== Funciones virtuales para sobreescribir por los hijos ========
    protected virtual float GetCost(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }

    protected virtual List<Vector3> GetConnections(Vector3 curr)
    {
        var neighbors = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                var neighbor = curr + new Vector3(x, 0, z);
                if (ObstacleManager.Instance.IsRightPos(neighbor))
                    neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    protected virtual float Heuristic(Vector3 current, Vector3 goal)
    {
        return Vector3.Distance(current, goal);
    }

    protected virtual bool IsSatisfied(Vector3 current, Vector3 goal)
    {
        return Vector3.Distance(current, goal) <= 0.8f;
    }

    protected virtual bool InView(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        return !Physics.Raycast(from, dir.normalized, dir.magnitude, PathfindingConstants.obsMask);
    }

    public virtual void Sleep() { }

}
