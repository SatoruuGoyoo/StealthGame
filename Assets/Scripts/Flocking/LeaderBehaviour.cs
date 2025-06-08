using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : FlockingBaseBehaviour
{
    public float timePrediction = 0.5f;

    private Pursuit _pursuit;
    private bool _isPursuit;

    private void Awake()
    {
        // Usamos constructor compatible con tu profe
        _pursuit = new Pursuit(transform, errorRange: 0, timePrediction: timePrediction);
    }

    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_isPursuit && _pursuit != null)
        {
            return _pursuit.GetDir() * multiplier;
        }

        return Vector3.zero;
    }

    public Transform Leader
    {
        set
        {
            var rb = value.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _pursuit.Target = rb;
                _isPursuit = true;
            }
            else
            {
                Debug.LogWarning("LeaderBehaviour: El líder no tiene Rigidbody.");
                _isPursuit = false;
            }
        }
    }

    public Rigidbody LeaderRb
    {
        set
        {
            if (value != null)
            {
                _pursuit.Target = value;
                _isPursuit = true;
            }
        }
    }
}
