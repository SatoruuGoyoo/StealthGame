using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerModel : MonoBehaviour, IMove, IAttack, ICrouch
{
    public float speed;
    Rigidbody _rb;
    Action _onAttack = delegate { };

    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    CapsuleCollider _collider;
    float _originalHeight;
    float _crouchHeight = 0.9f;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _originalHeight = _collider.height;
    }

    public virtual void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }

    public virtual void Attack()
    {
        _onAttack();
    }

    public void StartCrouch()
    {
        _collider.height = _crouchHeight;
    }

    public void StopCrouch()
    {
        _collider.height = _originalHeight;
    }
}
