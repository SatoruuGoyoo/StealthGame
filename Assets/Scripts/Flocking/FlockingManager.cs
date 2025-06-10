using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour, ISteering
{
    [Min(1)] public int maxBoids = 10;
    [Min(1)] public int radius = 10;
    public LayerMask boidMask;

    IFlocking[] _behaviours;
    IBoid _self;
    Collider[] _colls;
    List<IBoid> _boids;

    private LeaderBehaviour _leader;

    private void Start()
    {
        _behaviours = GetComponents<IFlocking>();
        _self = GetComponent<IBoid>();
        _colls = new Collider[maxBoids];
        _boids = new List<IBoid>();
    }

    public Vector3 GetDir()
    {
        _boids.Clear();
        int count = Physics.OverlapSphereNonAlloc(_self.Position, radius, _colls, boidMask);

        for (int i = 0; i < count; i++)
        {
            var boid = _colls[i].GetComponent<IBoid>();
            if (boid == null || boid == _self) continue;
            _boids.Add(boid);
        }

        Vector3 dir = Vector3.zero;

        // Logs por comportamiento individual
        foreach (var behaviour in _behaviours)
        {
            Vector3 contrib = behaviour.GetDir(_boids, _self);
            Debug.Log($"{_self} → {behaviour.GetType().Name} devuelve: {contrib}");
            dir += contrib;
        }

        // Seek al líder si no hay vecinos
        if (_boids.Count == 0 && _leader != null)
        {
            Vector3 seek = (_leader.transform.position - _self.Position).normalized;
            dir += seek * 1.5f;
        }

        // Log final del vector compuesto
        if (dir.sqrMagnitude < 0.01f)
        {
            Debug.Log($"{_self} → dir final de flocking es cero. Vecinos: {_boids.Count}, Leader: {_leader != null}");
        }
        else
        {
            Debug.Log($"{_self} → dir total final de flocking: {dir} con vecinos: {_boids.Count}, Leader asignado: {_leader != null}");
        }

        return dir.normalized;
    }

    public bool HasNeighbors() => _boids.Count > 0;

    public IBoid SetSelf { set => _self = value; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void SetLeader(LeaderBehaviour leader)
    {
        _leader = leader;

        foreach (var behaviour in _behaviours)
        {
            if (behaviour is FollowLeaderBehaviour follow)
                follow.SetLeader(leader.transform);
        }
    }
}
