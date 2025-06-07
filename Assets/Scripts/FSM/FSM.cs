using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    IState<T> _currState;

    private T _currentEnum;

    public FSM() { }
    public FSM(IState<T> curr)
    {
        SetInit(curr);
    }

    //Sets the initial state
    public void SetInit(IState<T> curr)
    {
        curr.StateMachine = this;
        _currState = curr;
        _currState.Enter();
    }
    public void OnExecute()
    {
        if (_currState != null)
            _currState.Execute();
    }
    public void OnFixExecute()
    {
        if (_currState != null)
            _currState.FixExecute();
    }

    //Transitions to a new state
    public void Transition(T input)
    {
        IState<T> newState = _currState.GetTransition(input);
        if (newState == null) return;

        if (newState == _currState)
        {
            Debug.Log("REINICIANDO estado actual: " + input);
            _currState.Exit();
            _currState.Enter();
            return;
        }

        newState.StateMachine = this;
        _currState.Exit();
        _currState = newState;
        _currentEnum = input;
        _currState.Enter();
    }

    public T CurrentStateEnum => _currentEnum;
}
