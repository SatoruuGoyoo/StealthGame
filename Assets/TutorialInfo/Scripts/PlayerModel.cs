using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(CapsuleCollider))]
public class PlayerModel : MonoBehaviour, IMove, IAttack, ICrouch
{
    public float speed;
    Rigidbody _rb;
    CapsuleCollider _collider;

    Action _onAttack = delegate { };
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    public bool IsCrouching { get; private set; } = false;

    float _originalHeight = 0.03636042f;
    Vector3 _originalCenter = new Vector3(0.0006558567f, 0.01688617f, 0f);

    [SerializeField] float crouchHeight = 0.02f;
    [SerializeField] Vector3 crouchCenter = new Vector3(0.0006558567f, 0.009f, 0f);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        // Guarda valores reales por si cambian en el prefab
        _originalHeight = _collider.height;
        _originalCenter = _collider.center;
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }

    public void Attack()
    {
        _onAttack();
    }

    public void StartCrouch()
    {
        IsCrouching = true;
        _collider.height = crouchHeight;
        _collider.center = crouchCenter;
    }

    public void StopCrouch()
    {
        IsCrouching = false;
        _collider.height = _originalHeight;
        _collider.center = _originalCenter;
    }
}
