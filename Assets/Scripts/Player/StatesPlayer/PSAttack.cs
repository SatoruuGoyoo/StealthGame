using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Estado del jugador realizando un ataque.

public class PSAttack<T> : PSBase<T>
{
    T _exitState;
    float _attackDuration = 0.3f; 
    float _timer;

    public PSAttack(T exitState)
    {
        _exitState = exitState;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("ATTACK: ENTER");
        _timer = _attackDuration;

        _attack.Attack(); 
    }

    public override void Execute()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            StateMachine.Transition(_exitState);
        }
    }
}
