using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol<T> : NPCBase<T>
{
    List<Transform> _patrolPoints;
    int _currentPatrolIndex = 0;
    [SerializeField] float _arriveTreshold = 1f;
    [SerializeField] float _waitTime = 1f;
    float _timer;
    bool _waitingatWaypoint = false;

    public NPCPatrol(List<Transform> patrolPoints)
    {

        _patrolPoints = patrolPoints;

    }
    public override void Execute()
    {
        base.Execute();
        if (_patrolPoints == null || _patrolPoints.Count == 0) return;

        var target = _patrolPoints[_currentPatrolIndex];
        Vector3 dir = target.position - _move.Position;

        if(_waitingatWaypoint)
        {
            _move.Move(Vector3.zero);
            _timer += Time.deltaTime;
            if (_timer >= _waitTime)
            {
                _waitingatWaypoint = false;
                _timer = 0f;
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
                target = _patrolPoints[_currentPatrolIndex];
                dir = target.position - _move.Position;
            }
            return;
        }

        if (dir.magnitude < _arriveTreshold)
        {
            _waitingatWaypoint = true;
            _move.Move(Vector3.zero);
            return;
        }

        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }
}
