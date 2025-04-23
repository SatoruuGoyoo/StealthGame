using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyModel : PlayerModel, IMove
{
    //[Header("Line Of Sight")]
    //[Range(1, 360)]
    //public float angle;
    //public float range;
    //public Action<bool> onChangeEntityUI = delegate { };


    //[Header("Enemy Layer")]
    //public LayerMask obstacleMask;

    //[Header("Enemy Target")]
    //[SerializeField] private Transform _target;

    //bool _isDetectingEntity;

    public float attackRange;
    public LayerMask enemyMask;

    ILook _look;

    //public Transform CheckTarget()
    //{
    //    return _target;
    //}

    protected override void Awake()
    {
        _look = GetComponent<ILook>();
        base.Awake();
    }

    public override void Attack()
    {
        var colls = Physics.OverlapSphere(Position, attackRange, enemyMask);
        for (int i = 0; i < colls.Length; i++)
        {
            GameObject.Destroy(colls[i].gameObject);
        }
        base.Attack();
    }

    //public bool DetectingEntity
    //{
    //    set
    //    {
    //        if (value != _isDetectingEntity) onChangeEntityUI(value);
    //        _isDetectingEntity = value;
    //    }
    //    get
    //    {
    //        return _isDetectingEntity;
    //    }
    //}

    public override void Move(Vector3 dir)
    {
        _look.LookDir(dir);
        base.Move(dir);
    }

    //private void OnDestroy()
    //{
    //    onChangeEntityUI = null;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, range);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
    //    Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);

    //}
}
