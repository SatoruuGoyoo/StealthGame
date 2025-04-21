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
        var attack = GetComponent<IAttack>();
        var crouchHandler = GetComponent<ICrouch>(); // <- ESTA es la interfaz

        var stateList = new List<PSBase<StateEnum>>();

        var idle = new PSIdle<StateEnum>(StateEnum.Walk);
        var walk = new PSWalk<StateEnum>(StateEnum.Idle);
        var crouch = new PSCrouch<StateEnum>(StateEnum.Idle); // <- ESTE es el estado

        // Transiciones
        idle.AddTransition(StateEnum.Walk, walk);
        idle.AddTransition(StateEnum.Crouch, crouch);

        walk.AddTransition(StateEnum.Idle, idle);
        walk.AddTransition(StateEnum.Crouch, crouch);

        crouch.AddTransition(StateEnum.Idle, idle);

        // Agregar estados a la lista
        stateList.Add(idle);
        stateList.Add(walk);
        stateList.Add(crouch);

        // Inicializar estados
        foreach (var state in stateList)
        {
            state.Initialize(move, look, attack, crouchHandler); // ¡acá se pasa la interfaz!
        }

        // Estado inicial
        _fsm.SetInit(idle);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _fsm.Transition(StateEnum.Crouch);
        }

        _fsm.OnExecute();
    }
}
