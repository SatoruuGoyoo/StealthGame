using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerView : MonoBehaviour, ILook
{
    [SerializeField] Animator _anim;
    Rigidbody _rb;
    public float speedRot = 10;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        GetComponent<IAttack>().OnAttack += OnSpinAnim;
    }

    public void Update()
    {
        UpdateMovementAnimations();
    }

    public void LookDir(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRot);
    }

    public void OnSpinAnim()
    {
        _anim.SetTrigger("Spin");
    }

    void UpdateMovementAnimations()
    {
        float vel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;
        _anim.SetFloat("Vel", vel);
        // "Crouch" lo maneja PSCrouch directamente
    }
}
