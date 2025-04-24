using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundRandom : MonoBehaviour
{
    public SoundDataBase dataBase;
    public AudioSource audioSource;
    Dictionary<RarityEnum, float> _weights;
    public float interval = 5f;

    private void Awake()
    {
        _weights = new Dictionary<RarityEnum, float>();
        {
            { RarityEnum.Common, 1f},
            { RarityEnum.Rare, 0.5f},
            { RarityEnum.UltraRare, 0.1f}
        };

        StartCoroutine(PlayRandomSoundLoop());
    }

    IEnumerator PlayRandomSoundLoop()
    {
        while (true)
        {
            GetRandomSound();
            yield return new WaitForSeconds(interval);
        } 
    }

    void GetRandomSound()
}
