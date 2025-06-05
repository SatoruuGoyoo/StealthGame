using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol<T> : StatePathfinding<T>
{
    private List<Vector3> _patrolPoints;
    private int _currentPatrolIndex = 0;

    private float _arriveThreshold = 1f;
    private float _waitTime = 1f;
    private float _timer;
    private bool _waitingAtWaypoint = false;

    private ILook _look;

    public NPCPatrol(Transform entity, IMove move, ILook look, Animator anim, List<Transform> patrolPoints)
        : base(entity, move, anim)
    {
        _look = look;

        _patrolPoints = new List<Vector3>();
        foreach (var p in patrolPoints)
            _patrolPoints.Add(Vector3Int.RoundToInt(p.position));
    }

    public override void Enter()
    {
        base.Enter();
        GoToNextPoint();
    }

    public override void Execute()
    {
        base.Execute();

        if (IsFinishPath && !_waitingAtWaypoint)
        {
            _timer = 0f;
            _waitingAtWaypoint = true;
        }

        if (_waitingAtWaypoint)
        {
            _move.Move(Vector3.zero);
            _timer += Time.deltaTime;

            if (_timer >= _waitTime)
            {
                _waitingAtWaypoint = false;
                GoToNextPoint();
            }
        }
    }

    private void GoToNextPoint()
    {
        var next = _patrolPoints[_currentPatrolIndex];
        SetPathTo(next);

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
    }
}
