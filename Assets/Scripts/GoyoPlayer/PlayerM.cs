using UnityEngine;

public class PlayerM : MonoBehaviour, IMove
{
    [SerializeField] private float speed;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.linearVelocity.y; // Preserve the Y velocity
        _rb.linearVelocity = dir;
    }


}
