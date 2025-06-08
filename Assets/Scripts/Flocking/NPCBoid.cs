using UnityEngine;

public class NPCBoid : MonoBehaviour, IBoid
{
    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;
}

