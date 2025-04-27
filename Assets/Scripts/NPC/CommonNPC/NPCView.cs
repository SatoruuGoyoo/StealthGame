using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCView : PlayerView
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _spottedClip;

    private NPCController _npcController;

    private void Awake()
    {
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
