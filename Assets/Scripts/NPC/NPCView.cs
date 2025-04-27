using UnityEngine;

public class NPCView : PlayerView
{
    [SerializeField] private LineOfSightMono _los;
    [SerializeField] private Transform _player;
    [SerializeField] private AudioSource _audioSource;
    private bool _hasSeenPlayer = false;

    private void Start()
    {
        if (_los == null)
        {
            _los = GetComponent<LineOfSightMono>();
        }
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        if (_player == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private new void Update()
    {
        base.Update();

        if (_player == null) return;

        bool seesPlayer = _los.LOS(_player);

        if (seesPlayer && !_hasSeenPlayer)
        {
            _hasSeenPlayer = true;
            PlaySpottedSound();
        }
        else if (!seesPlayer)
        {
            _hasSeenPlayer = false;
        }
    }

    private void PlaySpottedSound()
    {
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
