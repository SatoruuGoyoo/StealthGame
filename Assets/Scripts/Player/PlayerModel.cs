using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Responsable del comportamiento físico del jugador: movimiento, ataque, agacharse.
// Implementa las interfaces IMove, IAttack e ICrouch para integrarse a la FSM.

[RequireComponent(typeof(BoxCollider))]
public class PlayerModel : MonoBehaviour, IMove, IAttack, ICrouch
{
    [Header("Speed Settings")]
    public float speed;
    public float speedRot = 3f;


    Rigidbody _rb;
    BoxCollider _collider;

    Action _onAttack = delegate { };
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    public bool IsCrouching { get; private set; } = false;

    // Toma en cuenta la hitbox del player con sus medidas 
    Vector3 originalSize = new Vector3(0.006859852f, 0.03552359f, 0.00960762f);
    Vector3 originalCenter = new Vector3(-0.0002046084f, 0.01772303f, -0.0006253576f);

    // ajusto la hitbos del player cuando se agacha 
    [Header("Collider Settings")]
    [SerializeField] Vector3 crouchSize = new Vector3(0.006859852f, 0.02f, 0.00960762f);
    [SerializeField] Vector3 crouchCenter = new Vector3(-0.0002046084f, 0.0105f, -0.0006253576f);

    //rango de ataque 
     private float _attackRange = 1.5f; 

    public Vector3 Position => transform.position;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();

        originalSize = _collider.size;
        originalCenter = _collider.center;
    }

    public virtual void Move(Vector3 dir)
    {
        if (dir.magnitude > 1f)
        {
            dir.Normalize();
        }
        dir *= speed;
        dir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = dir;
    }

    public void LookDir(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.01f) return;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 8f);
    }




    public virtual void Attack()
    {
        _onAttack();

       
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("NPC2"))
            {
                Destroy(hit.gameObject);
                return; 
            }
        }

       
    }

    public void StartCrouch()
    {
        IsCrouching = true;
        _collider.size = crouchSize;
        _collider.center = crouchCenter;
    }

    public void StopCrouch()
    {
        IsCrouching = false;
        _collider.size = originalSize;
        _collider.center = originalCenter;
    }
}
