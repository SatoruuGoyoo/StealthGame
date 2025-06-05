//using System.Collections.Generic;
//using UnityEngine;


//// Enemy Evade Controller

//// Initilaize FSM - Decision Tree - Steering (EVADE)

//// This script controls the behavior of an NPC that can evade a target using a finite state machine (FSM) and a decision tree.

//public class EvadeNPCController : MonoBehaviour
//{
//    [Header("Waypoints")]
//    public List<Transform> _patrolPoints;

//    [Header("Common Settings")]
//    public Rigidbody target;
//    public Transform zone;
//    public float waitTime;

//    [Header("Evade Time")]
//    public float timePrediction;

//    private FSM<StateEnum> _fsm;
//    private NPCModel _model;
//    private LineOfSightMono _los;
//    private ITreeNode _root;
//    private ISteering _evadeSteering;

//    private ILook _look;

//    private bool _isEvading = false;

//    private float _timer = 0f;

//    private void Awake()
//    {
//        _model = GetComponent<NPCModel>();
//        _los = GetComponent<LineOfSightMono>();
//        _look = GetComponent<ILook>();

//    }

//    void Start()
//    {
//        InitializeSteerings();
//        InitializeFSM();
//        InitializeTree();
//    }

//    void Update()
//    {
//        if (target != null)
//        {
//            _fsm.OnExecute();
//            _root.Execute();
//            _timer += Time.deltaTime;
//        }
//    }

//    private void FixedUpdate()
//    {
//        _fsm.OnFixExecute();
//    }

//    void InitializeSteerings()
//    {
//        _evadeSteering = new Evade(_model.transform, target, 0, timePrediction);

//    }

//    void InitializeFSM()
//    {
//        _fsm = new FSM<StateEnum>();

//        // Create States
//        var idle = new NPCIdle<StateEnum>();
//        var attack = new NPCAttack<StateEnum>();
//        var evade = new NPCSteering<StateEnum>(_evadeSteering);
//        var goZone = new NPCChase<StateEnum>(zone);
//        var patrol = new NPCPatrol<StateEnum>(_patrolPoints);

//        var states = new List<PSBase<StateEnum>> { idle, attack, evade, goZone, patrol };

//        // Create Transitions
//        idle.AddTransition(StateEnum.Chase, evade);
//        idle.AddTransition(StateEnum.Patrol, patrol);

//        patrol.AddTransition(StateEnum.Chase, evade);
//        patrol.AddTransition(StateEnum.Idle, idle);

//        evade.AddTransition(StateEnum.GoZone, goZone);

//        goZone.AddTransition(StateEnum.Idle, idle);

//        foreach (var state in states)
//        {
//            state.Initialize(_model, _look, _model);
//        }

//        _fsm.SetInit(idle);
//    }

//    void InitializeTree()
//    {
//        var patrol = new ActionNode(() =>
//        {
//            _timer = 0f;
//            _fsm.Transition(StateEnum.Patrol);
//        });

//        var idle = new ActionNode(() =>
//        {
//            _timer = 0f;
//            _fsm.Transition(StateEnum.Idle);
//        });

//        var evadePlayer = new ActionNode(() =>
//        {
//            _isEvading = true;
//            _fsm.Transition(StateEnum.Chase);
//        });

//        var goToZone = new ActionNode(() =>
//        {
//            _fsm.Transition(StateEnum.GoZone);
//        });

//        // Decision Tree
//        var qSeeAlarm = new QuestionNode(QuestionSeeAlarm, goToZone, new ActionNode(() => { }));

//        var qIsEvading = new QuestionNode(() => _isEvading, qSeeAlarm,
//            new QuestionNode(QuestionTargetInView, evadePlayer,
//                new QuestionNode(QuestionWaitForTime, patrol, idle)
//            )
//        );

//        _root = qIsEvading;
//    }

//    bool QuestionTargetInView()
//    {
//        if (target == null) return false;
//        return _los.LOS(target.transform);
//    }

//    bool QuestionSeeAlarm()
//    {
//        if (zone == null) return false;
//        return _los.LOS(zone.transform);
//    }

//    bool QuestionWaitForTime()
//    {
//        return _timer >= waitTime;
//    }
//}
