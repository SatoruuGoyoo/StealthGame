using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerView : MonoBehaviour, ILook
{
    [SerializeField] Animator _anim;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _spottedClip;
    Rigidbody _rb;
    public float speedRot = 10;

    private NPCController _npcController;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        GetComponent<IAttack>().OnAttack += OnSpinAnim;

        _npcController = GetComponent<NPCController>();
        if (_npcController != null)
        {
            _npcController.OnTargetInView += HandleTargetInView;
        }

        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }

    public void Update()
    {
        UpdateMovementAnimations();
    }

    public void LookDir(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.001f) return; 

        Quaternion targetRotation = Quaternion.LookRotation(dir.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speedRot);
    }


    public void OnSpinAnim()
    {
        _anim.SetTrigger("Spin");
    }

    void UpdateMovementAnimations()
    {
        float vel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;
        _anim.SetFloat("Vel", vel);
      
    }

    private void HandleTargetInView(bool seesPlayer)
    {
        if (seesPlayer)
        {
            PlaySpottedSound();
        }
    }

    private void PlaySpottedSound()
    {
        if (_audioSource != null && _spottedClip != null)
        {
            _audioSource.PlayOneShot(_spottedClip);
        }
    }

    private void OnDestroy()
    {
        if (_npcController != null)
        {
            _npcController.OnTargetInView -= HandleTargetInView;
        }
    }
}
