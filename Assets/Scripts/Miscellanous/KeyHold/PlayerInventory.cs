using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private HashSet<string> keycards = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void CollectKeycard(string id)
    {
        keycards.Add(id);
    }

    public bool HasKeycard(string id)
    {
        return keycards.Contains(id);
    }
}
