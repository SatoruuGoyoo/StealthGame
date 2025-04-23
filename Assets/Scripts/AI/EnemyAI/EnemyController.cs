using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public Transform zone;
    FSM<StateEnum> _fsm;
    private EnemyModel _model;
    private LineOfSightMono _los;

    private void Awake()
    {
        _los = GetComponent<LineOfSightMono>();
        _model = GetComponent<EnemyModel>();

    }

    private void Start()
    {
        InitializedFSM();
    }

    void Update()
    {
        _fsm.OnExecute();
    }

    private void FixedUpdate()
    {
        _fsm.OnFixExecute();
    }

    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>();

        var idle = new NPCIdle<StateEnum>();
        var attack = new NPCAttack<StateEnum>();
        var chase = new NPCChase<StateEnum>(target);
        var goZone = new NPCChase<StateEnum>(zone);

        var stateList = new List<PSBase<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(chase);
        stateList.Add(goZone);

        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Spin, attack);
        idle.AddTransition(StateEnum.GoZone, goZone);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.GoZone, goZone);

        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Spin, attack);
        chase.AddTransition(StateEnum.GoZone, goZone);

        goZone.AddTransition(StateEnum.Chase, chase);
        goZone.AddTransition(StateEnum.Spin, attack);
        goZone.AddTransition(StateEnum.Idle, idle);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(_model, look, _model);
        }

        _fsm.SetInit(idle);
    }

    /*private void Update()
    {
        var target = _model.CheckTarget();
        if (_los.LoS(_model.transform, _model.CheckTarget(), _model.range, _model.angle, _model.obstacleMask))
        {
            print("Target in range and angle, no obstacle");
            _model.DetectingEntity = true;
        }
        else
        {
            print("Target out of range or angle, or obstacle in the way");
            _model.DetectingEntity = false;
        }
        
    }*/



}
