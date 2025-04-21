using UnityEngine;

public class PlayerV : MonoBehaviour, ILook
{

    [SerializeField] private float speedRotation = 10f;

    public void LookDirection(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRotation);
    }


}
