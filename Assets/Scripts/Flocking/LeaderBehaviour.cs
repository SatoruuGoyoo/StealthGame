using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : FlockingBaseBehaviour
{
    public float timePrediction;

    private Seek _seek;
    private Pursuit _pursuit;
    private bool _isPursuit;

    private void Awake()
    {
        _pursuit = new Pursuit(transform, 0, timePrediction);
        _seek = new Seek(transform);
    }

    // Calculates the movement direction for the leader
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_isPursuit && _pursuit != null)
        {
            Vector3 dir = _pursuit.GetDir();
            return dir * multiplier;
        }

        return _seek.GetDir() * multiplier;
    }

    // Called externally to get the movement direction
    public Vector3 GetDir()
    {
        return GetRealDir(null, null);
    }

    // Sets the leader target using a Transform
    public Transform Leader
    {
        set
        {
            var rb = value.GetComponent<Rigidbody>();
            if (rb)
            {
                _pursuit.Target = rb;
                _isPursuit = true;
            }
            else
            {
                _seek.Target = value;
                _isPursuit = false;
            }
        }
    }

    // Sets the leader target using a Rigidbody
    public Rigidbody LeaderRb
    {
        set
        {
            _pursuit.Target = value;
            _isPursuit = true;
        }
    }
}
