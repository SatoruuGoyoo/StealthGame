using System.Collections.Generic;
using UnityEngine;

public class EvadeNPCController : MonoBehaviour
{
    public Rigidbody target;
    public Transform zone;
    public float timePrediction;

    private FSM<StateEnum> _fsm;
    private NPCModel _model;
    private LineOfSightMono _los;
    private ITreeNode _root;
    private ISteering _evadeSteering;
    private ISteering _pursuitSteering;
    private ILook _look;

    private bool _isEvading = false;
    private bool _isGoingToZone = false;

    private void Awake()
    {
        _model = GetComponent<NPCModel>();
        _los = GetComponent<LineOfSightMono>();
        _look = GetComponent<ILook>();
    }

    void Start()
    {
        InitializeSteerings();
        InitializeFSM();
        InitializeTree();
    }

    void Update()
    {
        if (target != null)
        {
            _fsm.OnExecute();
            _root.Execute();
        }
    }

    private void FixedUpdate()
    {
        _fsm.OnFixExecute();
    }

    void InitializeSteerings()
    {
        _evadeSteering = new Evade(_model.transform, target, 0, timePrediction);
        _pursuitSteering = new Pursuit(_model.transform, target, 0, timePrediction);
    }

    void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();

        var idle = new NPCIdle<StateEnum>();
        var attack = new NPCAttack<StateEnum>();
        var evade = new NPCSteering<StateEnum>(_evadeSteering); 
        var goZone = new NPCChase<StateEnum>(zone); 

        var states = new List<PSBase<StateEnum>> { idle, attack, evade, goZone };

        idle.AddTransition(StateEnum.Chase, evade);
        idle.AddTransition(StateEnum.GoZone, goZone);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, evade);
        attack.AddTransition(StateEnum.GoZone, goZone);

        evade.AddTransition(StateEnum.GoZone, goZone); 

        goZone.AddTransition(StateEnum.Idle, idle);

        foreach (var state in states)
        {
            state.Initialize(_model, _look, _model);
        }

        _fsm.SetInit(idle);
    }

    void InitializeTree()
    {
        var goToZone = new ActionNode(() =>
        {
            Debug.Log("Transición a GoZone");
            _isGoingToZone = true;
            _fsm.Transition(StateEnum.GoZone);
        });

        var evadePlayer = new ActionNode(() =>
        {
            Debug.Log("Transición a Chase (Evade)");
            _isEvading = true;
            _fsm.Transition(StateEnum.Chase);
        });



        var qSeeAlarm = new QuestionNode(QuestionSeeAlarm, goToZone, new ActionNode(() => { Debug.Log("Sigo evadiendo"); }));

        var qIsEvading = new QuestionNode(() => _isEvading, qSeeAlarm,
            new QuestionNode(QuestionTargetInView, evadePlayer, new ActionNode(() => { Debug.Log("Sigo en Idle"); })));

        _root = qIsEvading;
    }

    bool QuestionTargetInView()
    {
        if (target == null) return false;
        return _los.LOS(target.transform);
    }

    bool QuestionSeeAlarm()
    {
        if (zone == null) return false;
        return _los.LOS(zone.transform);
    }
}
