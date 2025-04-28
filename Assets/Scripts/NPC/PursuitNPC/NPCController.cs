using System.Collections.Generic;
using UnityEngine;

// Enemy NPC Controller
// Initilaize FSM - Decision Tree - Steering (Pursuit)
// 

public class NPCController : MonoBehaviour
{
    [Header("NPC Waypoints")]
    public List<Transform> _patrolPoints;

    [Header("Pursuit Time")]
    public float timePrediction;

    [Header("NPC Common Settings")]
    public Rigidbody target;
    public float _waitTime;
    public float _timer;

     FSM<StateEnum> _fsm;
     NPCModel _model;
     LineOfSightMono _los;
     ITreeNode _root;
     ISteering _steering;

    public event System.Action<bool> OnTargetInView;

     bool previousLOSState = false;

    public StateEnum CurrentStateEnum { get; protected set; }

    private void Awake()
    {
        _model = GetComponent<NPCModel>();
        _los = GetComponent<LineOfSightMono>();
    }

    void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
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

    void InitializedSteering()
    {
        var pursuit = new Pursuit(_model.transform, target, 0, timePrediction);
        var evade = new Evade(_model.transform, target, 0, timePrediction);
        if (this.gameObject.tag == "NPC1")
        {
            _steering = pursuit;
        }
        else
        {
            _steering = evade;
        }
    }

    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var look = GetComponent<ILook>();

        // Create States
        var idle = new NPCIdle<StateEnum>();
        var attack = new NPCAttack<StateEnum>();
        var chase = new NPCSteering<StateEnum>(_steering);
        var patrol = new NPCPatrol<StateEnum>(_patrolPoints);  

        // Add to List
        var stateList = new List<PSBase<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(chase);
        stateList.Add(patrol);

        // Create Transitions
        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Attack, attack);
        idle.AddTransition(StateEnum.Patrol, patrol);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.Patrol, patrol);

        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.Patrol, patrol);

        patrol.AddTransition(StateEnum.Idle, idle);
        patrol.AddTransition(StateEnum.Chase, chase);
        patrol.AddTransition(StateEnum.Attack, attack);
      

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(_model, look, _model);
        }

        _fsm.SetInit(idle);
    }

    void InitializedTree()
    {
        var idle = new ActionNode(() =>
        {
            _timer = 0f;
            _fsm.Transition(StateEnum.Idle);
        });
        var attack = new ActionNode(() =>
        {
           
            _fsm.Transition(StateEnum.Attack);
        });
        var chase = new ActionNode(() =>
        {
            
            _fsm.Transition(StateEnum.Chase);
        });
        var patrol = new ActionNode(() =>
        {
            _fsm.Transition(StateEnum.Patrol);
        });
        var waitForTime = new QuestionNode(  // Wait time achieved
            QuestionWaitForTime,
            patrol, // True? = Patrol
            idle // False? = Idle
        );

        var qCanAttack = new QuestionNode(QuestionCanAttack, attack, chase);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, waitForTime);

        _root = qTargetInView;
    }

    bool QuestionCanAttack()
    {
        return Vector3.Distance(_model.Position, target.position) <= _model.attackRange;
    }
    bool QuestionWaitForTime()
    {
        return _timer >= _waitTime;
    }

    bool QuestionTargetInView()
    {
        if (target == null) return false;

        bool currentLOS = _los.LOS(target.transform);

        if (currentLOS != previousLOSState)
        {
            OnTargetInView?.Invoke(currentLOS);
            previousLOSState = currentLOS;
        }

        return currentLOS;
    }
}
