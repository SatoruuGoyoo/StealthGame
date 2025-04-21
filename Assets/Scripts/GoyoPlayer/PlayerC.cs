using UnityEngine;

public class PlayerC : MonoBehaviour
{
    IMove _move;
    ILook _look;

    private void Awake()
    {
        _move = GetComponent<IMove>();
        _look = GetComponent<ILook>();
    }

    private void Update()
    {
        var h = Input.GetAxis("Horizontal"); // X
        var v = Input.GetAxis("Vertical"); // Z

        var dir = new Vector3(h, 0, v);
        _move.Move(dir);
        if( h != 0 || v != 0)
        {
            _look.LookDirection(dir);
        }

        
    }
}
