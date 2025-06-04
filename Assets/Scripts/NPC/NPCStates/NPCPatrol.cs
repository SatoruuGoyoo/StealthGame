using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// NPC Patrol State
// NPC looks for waypoints to patrol - Waits 1f in every WYP

public class NPCPatrol<T> : NPCBase<T>
{
    private WeightedPatrolPointManager _patrolPointManager;
    private Queue<Vector2Int> _currentPath = new Queue<Vector2Int>();
    private float _arriveThreshold = 0.2f;
    private float _waitTime = 1f;
    private float _waitTimer = 0f;
    private bool _waiting = false;

    public NPCPatrol(WeightedPatrolPointManager patrolPointManager, float waitTime = 1f)
    {
        _patrolPointManager = patrolPointManager;
        _waitTime = waitTime;
    }

    public override void Enter()
    {
        base.Enter();
        GenerateNewPath();
    }

    public override void Execute()
    {
        base.Execute();

        if (_waiting)
        {
            _move.Move(Vector3.zero);
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _waitTime)
            {
                _waitTimer = 0f;
                _waiting = false;
                GenerateNewPath();
            }
            return;
        }

        if (_currentPath.Count == 0)
        {
            _waiting = true;
            return;
        }

        Vector2Int next = _currentPath.Peek();
        Vector3 worldPos = new Vector3(next.x, 0f, next.y);
        Vector3 dir = worldPos - _move.Position;

        if (dir.magnitude <= _arriveThreshold)
        {
            _currentPath.Dequeue();
        }
        else
        {
            _move.Move(dir.normalized);
            _look.LookDir(dir.normalized);
        }
    }

    private void GenerateNewPath()
    {
        Vector2Int start = new Vector2Int(Mathf.RoundToInt(_move.Position.x), Mathf.RoundToInt(_move.Position.z));
        Vector2Int target = _patrolPointManager.GetRandomPoint();

        List<Vector2Int> path = ASTAR.Run(
            start,
            t => t == target,
            GridManager.Instance.GetNeighbours,
            GridManager.Instance.GetCost,
            t => Vector2Int.Distance(t, target)
        );

        _currentPath.Clear();
        foreach (var node in path)
            _currentPath.Enqueue(node);
    }
}
