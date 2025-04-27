using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    
    static int _attackMouseButton = 1;
    static KeyCode _crouch = KeyCode.LeftControl;


    public static bool GetMouseAttack()
    {
        return Input.GetMouseButtonDown(_attackMouseButton);
    }

    

    public static bool GetKeyCrouch()
    {
        return Input.GetKey(_crouch);
    }


    public static Vector2 GetMove()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
