using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Estado del jugador agachado.

public class PSCrouch<T> : PSBase<T>
{
    T _exitState;
    ICrouch _crouch;

    public PSCrouch(T exitState)
    {
        _exitState = exitState;
    }

    public override void Initialize(params object[] p)
    {
        base.Initialize(p);
        _crouch = p[3] as ICrouch;
    }

    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector3.zero);
        _crouch?.StartCrouch();

        if (_look is MonoBehaviour view && view.TryGetComponent(out Animator anim))
        {
            anim.SetBool("Crouch", true);
        }
    }

    public override void Execute()
    {
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            StateMachine.Transition(_exitState);
            return;
        }

        var dir = new Vector3(InputManager.GetMove().x, 0, InputManager.GetMove().y);
        _move.Move(dir * 0.5f); // Movimiento reducido

        if (dir != Vector3.zero)
        {
            _look.LookDir(dir);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _crouch?.StopCrouch();

        if (_look is MonoBehaviour view && view.TryGetComponent(out Animator anim))
        {
            anim.SetBool("Crouch", false);
        }
    }
}
