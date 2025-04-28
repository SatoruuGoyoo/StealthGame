using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores sound clips in a dictionary based on their rarity
public class SoundDataBase : MonoBehaviour
{
    public Dictionary<RarityEnum, AudioClip[]> sounds = new Dictionary<RarityEnum, AudioClip[]>();

    private void Awake()
    {
        // Load sound clips from Resources folder and categorize them by rarity
        var common = Resources.LoadAll<AudioClip>("Sounds/Common");
        var rare = Resources.LoadAll<AudioClip>("Sounds/Rare");
        var ultraRare = Resources.LoadAll<AudioClip>("Sounds/UltraRare");

        sounds.Add(RarityEnum.Common, common);
        sounds.Add(RarityEnum.Rare, rare);
        sounds.Add(RarityEnum.UltraRare, ultraRare);
    }
}
