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
        //Initialize the weights for each rarity
        _weights = new Dictionary<RarityEnum, float>()
        {
            { RarityEnum.Common, 1f },
            { RarityEnum.Rare, 0.5f },
            { RarityEnum.UltraRare, 0.1f }
        };

        //Start the loop that plays sounds
        StartCoroutine(PlayRandomSoundLoop());
    }

    //Continously play the sounds at set intervals
    IEnumerator PlayRandomSoundLoop()
    {
        while (true)
        {
            GetRandomSound();
            yield return new WaitForSeconds(interval);
        } 
    }

    //Selects a random sound based on the rarity weights
    void GetRandomSound()
    {
        RarityEnum rarity = GetRandomRarityByWeight();
        SetSound(rarity);
    }

    //Picks and plays the sound of the selected rarity
    void SetSound(RarityEnum rarity)
    {
        if (!dataBase || !dataBase.sounds.ContainsKey(rarity)) return;

        AudioClip[] clips = dataBase.sounds[rarity];
        if (clips.Length == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        audioSource.clip = clips[randomIndex];
        audioSource.Play();
    }

    //Randomly selects a rarity based on the weights
    RarityEnum GetRandomRarityByWeight()
    {
        float total = 0;
        foreach (var item in _weights)
        {
            total += item.Value;
        }

        float randomValue = UnityEngine.Random.value * total;
        foreach (var item in _weights)
        {
            if (randomValue < item.Value)
                return item.Key;
            randomValue -= item.Value;
        }

        return RarityEnum.Common;
    }
}
