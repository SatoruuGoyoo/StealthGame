using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[RequireComponent(typeof(BoxCollider))]
public class PlayerModel : MonoBehaviour, IMove, IAttack, ICrouch
{
    public float speed;
    Rigidbody _rb;
    BoxCollider _collider;

    Action _onAttack = delegate { };
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    public bool IsCrouching { get; private set; } = false;

    Vector3 originalSize = new Vector3(0.006859852f, 0.03552359f, 0.00960762f);
    Vector3 originalCenter = new Vector3(-0.0002046084f, 0.01772303f, -0.0006253576f);

    [SerializeField] Vector3 crouchSize = new Vector3(0.006859852f, 0.02f, 0.00960762f);
    [SerializeField] Vector3 crouchCenter = new Vector3(-0.0002046084f, 0.0105f, -0.0006253576f);

     private float attackRange = 1.5f; // Nuevo: rango de ataque para encontrar enemigos

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

    public virtual void Attack()
    {
        _onAttack(); // Disparar animaciones primero

        // Buscar enemigos cerca
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("NPC2"))
            {
                Destroy(hit.gameObject);
                Debug.Log("¡Enemigo destruido con el ataque!");
                return; // Solo mata uno por ataque
            }
        }

        Debug.Log("No hay enemigos en el rango de ataque.");
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
