using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Rigidbody target;
    //public Transform zone;
    public float timePrediction;
    public float _waitTime;
    public float _timer;
    public List<Transform> _patrolPoints;
    public FSM<StateEnum> _fsm;
    protected NPCModel _model;
    protected LineOfSightMono _los;
    protected ITreeNode _root;
    protected ISteering _steering;

    public event System.Action<bool> OnTargetInView;

    protected bool previousLOSState = false;

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

    protected virtual void InitializedSteering()
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

    protected virtual void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var look = GetComponent<ILook>();

        // Create States
        var idle = new NPCIdle<StateEnum>();
        var attack = new NPCAttack<StateEnum>();
        var chase = new NPCSteering<StateEnum>(_steering);
        //var goZone = new NPCChase<StateEnum>(zone);
        var patrol = new NPCPatrol<StateEnum>(_patrolPoints);  

        // Add to List
        var stateList = new List<PSBase<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(chase);
        //stateList.Add(goZone);
        stateList.Add(patrol);

        // Create Transitions
        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Attack, attack);
        //idle.AddTransition(StateEnum.GoZone, goZone);
        idle.AddTransition(StateEnum.Patrol, patrol);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        //attack.AddTransition(StateEnum.GoZone, goZone);
        attack.AddTransition(StateEnum.Patrol, patrol);

        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Attack, attack);
        //chase.AddTransition(StateEnum.GoZone, goZone);
        chase.AddTransition(StateEnum.Patrol, patrol);

        //goZone.AddTransition(StateEnum.Chase, chase);
        //goZone.AddTransition(StateEnum.Attack, attack);
        //goZone.AddTransition(StateEnum.Idle, idle);
        //goZone.AddTransition(StateEnum.Patrol, patrol);

        patrol.AddTransition(StateEnum.Idle, idle);
        patrol.AddTransition(StateEnum.Chase, chase);
        patrol.AddTransition(StateEnum.Attack, attack);
        //patrol.AddTransition(StateEnum.GoZone, goZone);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(_model, look, _model);
        }

        _fsm.SetInit(idle);
    }

    protected virtual void InitializedTree()
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
        //var goZone = new ActionNode(() =>
        //{
        //    Debug.Log("Transición a GoZone");
        //    _fsm.Transition(StateEnum.GoZone);
        //});
        var patrol = new ActionNode(() =>
        {
            Debug.Log("Transición a Patrol");
            _fsm.Transition(StateEnum.Patrol);
        });
        var waitForTime = new QuestionNode(
            QuestionWaitForTime,
            patrol,
            idle
        );

        var qCanAttack = new QuestionNode(QuestionCanAttack, attack, chase);
        //var qGoToZone = new QuestionNode(QuestionGoToZone, goZone, idle);
        var qTargetInView = new QuestionNode(QuestionTargetInView, qCanAttack, waitForTime);

        _root = qTargetInView;
    }

    protected bool QuestionCanAttack()
    {
        return Vector3.Distance(_model.Position, target.position) <= _model.attackRange;
    }
    protected bool QuestionWaitForTime()
    {
        return _timer >= _waitTime;
    }

    //bool QuestionGoToZone()
    //{
    //    return Vector3.Distance(_model.transform.position, zone.transform.position) > 0.25f;
    //}

    protected bool QuestionTargetInView()
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
