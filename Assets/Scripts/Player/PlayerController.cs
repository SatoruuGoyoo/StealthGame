using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    FSM<StateEnum> _fsm;

    private void Awake()
    {
        InitializeFSM();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();
        var attackHandler = GetComponent<IAttack>(); // Renombrado correctamente
        var crouchHandler = GetComponent<ICrouch>();

        var stateList = new List<PSBase<StateEnum>>();

        var idle = new PSIdle<StateEnum>(StateEnum.Walk);
        var walk = new PSWalk<StateEnum>(StateEnum.Idle);
        var crouch = new PSCrouch<StateEnum>(StateEnum.Idle);
        var attackState = new PSAttack<StateEnum>(StateEnum.Idle); // También renombrado

        idle.AddTransition(StateEnum.Walk, walk);
        idle.AddTransition(StateEnum.Crouch, crouch);
        idle.AddTransition(StateEnum.Attack, attackState); // Ahora correcto

        walk.AddTransition(StateEnum.Idle, idle);
        walk.AddTransition(StateEnum.Crouch, crouch);
        walk.AddTransition(StateEnum.Attack, attackState);

        crouch.AddTransition(StateEnum.Idle, idle);

        stateList.Add(idle);
        stateList.Add(walk);
        stateList.Add(crouch);
        stateList.Add(attackState); // Correcto aquí también

        foreach (var state in stateList)
        {
            state.Initialize(move, look, attackHandler, crouchHandler); // Usamos el handler
        }

        _fsm.SetInit(idle);
    }

    private void Update()
    {
        if (InputManager.GetMouseAttack())
        {
            _fsm.Transition(StateEnum.Attack);
        }
        else if (InputManager.GetKeyCrouch())
        {
            _fsm.Transition(StateEnum.Crouch);
        }

        _fsm.OnExecute();
    }
}
