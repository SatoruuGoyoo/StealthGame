using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSWalk<T> : PSBase<T>
{
    T _inputToWalk;
    public PSWalk(T inputToIdle)
    {
        _inputToWalk = inputToIdle;
    }
    public override void Enter()
    {
        base.Enter();
        
    }
    public override void Execute()
    {
        Vector3 dir = new Vector3(InputManager.GetMove().x, 0, InputManager.GetMove().y);

        if (dir != Vector3.zero)
        {
            dir = dir.normalized; 
            _move.Move(dir);
            _look.LookDir(dir);
        }
        else
        {
            StateMachine.Transition(_inputToWalk);
        }
    }

}
