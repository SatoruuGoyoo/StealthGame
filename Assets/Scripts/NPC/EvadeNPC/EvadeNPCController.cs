using System.Collections.Generic;
using UnityEngine;

public class EvadeNPCController : MonoBehaviour
{
    public Rigidbody target;
    public Transform zone;
    public Transform patrolArea;
    public float timePrediction;
    public float waitTime;

    private FSM<StateEnum> _fsm;
    private NPCModel _model;
    private LineOfSightMono _los;
    private ITreeNode _root;
    private ISteering _evadeSteering;
    private ISteering _pursuitSteering;
    private BoxCollider _patrolAreaCollider;
    private ILook _look;

    private bool _isEvading = false;

    private float _timer = 0f;

    private void Awake()
    {
        _model = GetComponent<NPCModel>();
        _los = GetComponent<LineOfSightMono>();
        _look = GetComponent<ILook>();
        _patrolAreaCollider = patrolArea.GetComponent<BoxCollider>();
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
            _timer += Time.deltaTime;
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
        var patrol = new NPCPatrol<StateEnum>(_patrolAreaCollider);

        var states = new List<PSBase<StateEnum>> { idle, attack, evade, goZone, patrol };

        // Transiciones
        idle.AddTransition(StateEnum.Chase, evade);
        idle.AddTransition(StateEnum.Patrol, patrol);

        patrol.AddTransition(StateEnum.Chase, evade);
        patrol.AddTransition(StateEnum.Idle, idle);

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
        var patrol = new ActionNode(() =>
        {
            Debug.Log("Transición a Patrol");
            _timer = 0f;
            _fsm.Transition(StateEnum.Patrol);
        });

        var idle = new ActionNode(() =>
        {
            Debug.Log("Transición a Idle");
            _timer = 0f;
            _fsm.Transition(StateEnum.Idle);
        });

        var evadePlayer = new ActionNode(() =>
        {
            Debug.Log("Transición a Evade (Chase)");
            _isEvading = true;
            _fsm.Transition(StateEnum.Chase);
        });

        var goToZone = new ActionNode(() =>
        {
            Debug.Log("Transición a GoZone");
            _fsm.Transition(StateEnum.GoZone);
        });

        // Árbol de decisiones
        var qSeeAlarm = new QuestionNode(QuestionSeeAlarm, goToZone, new ActionNode(() => { Debug.Log("Sigo evadiendo"); }));

        var qIsEvading = new QuestionNode(() => _isEvading, qSeeAlarm,
            new QuestionNode(QuestionTargetInView, evadePlayer,
                new QuestionNode(QuestionWaitForTime, patrol, idle)
            )
        );

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

    bool QuestionWaitForTime()
    {
        return _timer >= waitTime;
    }
}
