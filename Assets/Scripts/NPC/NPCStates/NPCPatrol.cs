using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCPatrol<T> : StatePathfinding<T>
{
    private List<Vector3> _patrolPoints;
    private int _currentPatrolIndex = 0;

    private float _arriveThreshold = 1f;
    private float _waitTime = 1f;
    private float _timer;
    private bool _waitingAtWaypoint = false;

    private ILook _look;
    private Transform _fakeTarget;


    private static Transform CreateFakeTarget()
    {
        var go = new GameObject("FakeTarget_Patrol");
        go.hideFlags = HideFlags.HideInHierarchy;
        return go.transform;
    }

    public NPCPatrol(Transform entity, IMove move, ILook look, Animator anim, List<Transform> patrolPoints)
    : base(entity, move, anim, null)
    {
        _look = look;

        _fakeTarget = CreateFakeTarget();
        base._target = _fakeTarget;

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

    public override void Exit()
    {
        base.Exit();
        if (_fakeTarget != null)
            GameObject.Destroy(_fakeTarget.gameObject);
    }


    private void GoToNextPoint()
    {
        var next = _patrolPoints[_currentPatrolIndex];
        _target.position = next;

        SetPathAStarPlusVector();

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
    }
}
