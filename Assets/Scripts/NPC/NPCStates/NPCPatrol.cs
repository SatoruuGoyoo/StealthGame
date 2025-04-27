using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol<T> : NPCBase<T>
{
    BoxCollider _patrolAreaCollider;
    Vector3 _targetPoint;
    bool _hasTarget;
    [SerializeField] float _arriveTreshold = 0.5f;
    [SerializeField] float _waitTime = 2f;
    float _timer;
    bool _waitingTime = false;

    public NPCPatrol(BoxCollider area)
    {
     
        _patrolAreaCollider = area;

    }
    public override void Execute()
    {
        base.Execute();
        if(_patrolAreaCollider == null) return;

        if(_waitingTime)
        {
            _move.Move(Vector3.zero);
            _timer += Time.deltaTime;
            if (_timer >= _waitTime)
            {
                _waitingTime = false;
                _timer = 0f;
                _hasTarget = false;

            }
            return;
        }

        if (!_hasTarget)
        {
            _targetPoint = GetRandomPointInArea();
            _hasTarget = true;
        }

        Vector3 dir = _targetPoint - _move.Position;

        if (dir.magnitude < _arriveTreshold)
        {
            _waitingTime = true;
            _move.Move(Vector3.zero);

        }

        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
    }

    private Vector3 GetRandomPointInArea()
    {
        Vector3 center = _patrolAreaCollider.center + _patrolAreaCollider.transform.position;
        Vector3 size = _patrolAreaCollider.size * 1f;

        Vector3 randomPoint; 
        do
        {
            float randomX = Random.Range(-size.x, size.x);
            float randomZ = Random.Range(-size.z, size.z);
            float randomY = Random.Range(-size.y, size.y);

            randomPoint = new Vector3(randomX, randomY, randomZ) + center; 
        }
        while (Vector3.Distance(randomPoint, _move.Position) < 1.5f);

        return randomPoint;
    }
}
