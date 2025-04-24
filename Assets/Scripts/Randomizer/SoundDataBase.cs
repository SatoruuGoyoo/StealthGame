using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDataBase : MonoBehaviour
{
    public Dictionary<RarityEnum, AudioClip[]> sounds = new Dictionary<RarityEnum, AudioClip[]>();

    private void Awake()
    {
        var common = Resources.LoadAll<AudioClip>("Sounds/Common");
        var rare = Resources.LoadAll<AudioClip>("Sounds/Rare");
        var ultraRare = Resources.LoadAll<AudioClip>("Sounds/UltraRare");

        sounds.Add(RarityEnum.Common, common);
        sounds.Add(RarityEnum.Rare, rare);
        sounds.Add(RarityEnum.UltraRare, ultraRare);
    }
}
