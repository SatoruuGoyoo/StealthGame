using System;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyModel : MonoBehaviour
{
    [Header("Line Of Sight")]
    [Range(1, 360)]
    public float angle;
    public float range;
    public Action<bool> onChangeEntityUI = delegate { };


    [Header("Enemy Layer")]
    public LayerMask obstacleMask;

    [Header("Enemy Target")]
    [SerializeField] private Transform _target;

    bool _isDetectingEntity;

    public Transform CheckTarget()
    {
        return _target;
    }

    public bool DetectingEntity
    {
        set
        {
            if (value != _isDetectingEntity) onChangeEntityUI(value);
            _isDetectingEntity = value;
        }
        get
        {
            return _isDetectingEntity;
        }
    }

    private void OnDestroy()
    {
        onChangeEntityUI = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);

    }
}
