using System.Collections.Generic;
using UnityEngine;

// Enemy NPC Controller
// Initilaize FSM - Decision Tree - Steering (Pursuit)
// 

public class NPCController : MonoBehaviour
{
    [Header("NPC Waypoints")]
    public List<Transform> _patrolPoints;

    //[Header("Pursuit Time")]
    //public float timePrediction;

    [Header("NPC Common Settings")]
    public Rigidbody target;

    FSM<StateEnum> _fsm;
    NPCModel _model;
     LineOfSightMono _los;
     ITreeNode _root;
     ISteering _steering;

    public event System.Action<bool> OnTargetInView;

    private NPCMemory _memory = new NPCMemory();


    bool previousLOSState = false;

    public StateEnum CurrentStateEnum { get; protected set; }

    private void Awake()
    {
        _model = GetComponent<NPCModel>();
        _los = GetComponent<LineOfSightMono>();
    }

    void Start()
    {
        //InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    void Update()
    {
        _fsm.OnExecute();
        _root.Execute(); 
    }

    private void FixedUpdate()
    {
        _fsm.OnFixExecute();
    }

    //void InitializedSteering()
    //{
    //    var pursuit = new Pursuit(_model.transform, target, 0, timePrediction);
    //    var evade = new Evade(_model.transform, target, 0, timePrediction);
    //    //if (this.gameObject.tag == "NPC1")
    //    //{
    //    //    _steering = pursuit;
    //    //}
    //    //else
    //    //{
    //    //    _steering = evade;
    //    //}
    //}

    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var look = GetComponent<ILook>();
        var anim = GetComponent<Animator>();

        // Create States
        var idle = new NPCIdle<StateEnum>();
        var attack = new NPCAttack<StateEnum>();

        var patrol = new NPCPatrol<StateEnum>(_model.transform, _model, look, anim, _patrolPoints);
        var chase = new NPCChase<StateEnum>(_model.transform, _model, look, anim, target.transform);
        var search = new NPCSearch<StateEnum>(_model.transform, _model, anim, _memory);


        // Add to List
        var stateList = new List<IState<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(patrol);
        stateList.Add(chase);
        stateList.Add(search);

        // Create Transitions
        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Attack, attack);
        idle.AddTransition(StateEnum.Patrol, patrol);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.Patrol, patrol);

        search.AddTransition(StateEnum.Chase, chase);
        search.AddTransition(StateEnum.Patrol, patrol);

        //Chase
        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.Patrol, patrol);
        chase.AddTransition(StateEnum.Search, search);

        //Patrol
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
        var patrol = new ActionNode(() => _fsm.Transition(StateEnum.Patrol));
        var search = new ActionNode(() => _fsm.Transition(StateEnum.Search));
        var chase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
        var attack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

        // Si puede atacar → atacar
        var qAttack = new QuestionNode(QuestionCanAttack, attack, chase);

        // Si puede ver al jugador → pregunta si puede atacar
        var qChase = new QuestionNode(QuestionCanSeePlayer, qAttack, search);

        // Si no puede ver → pregunta si está buscando
        var qSearchRequest = new QuestionNode(QuestionSearchRequested, search, patrol);
        var qIsSearching = new QuestionNode(QuestionIsSearching, search, qSearchRequest);

        _root = new QuestionNode(QuestionCanSeePlayer, qAttack, qIsSearching);
    }


    bool QuestionCanAttack()
    {
        return Vector3.Distance(_model.Position, target.position) <= _model.attackRange;
    }

    bool QuestionCanSeePlayer()
    {
        if (target == null) return false;

        bool currentLOS = _los.LOS(target.transform);

        if (currentLOS != previousLOSState)
        {
            OnTargetInView?.Invoke(currentLOS);
            previousLOSState = currentLOS;

            if (!currentLOS)
            {
                _memory.SearchRequested = true;
            }
        }

        if (currentLOS)
            _memory.UpdateLastSeen();

        return _memory.ShouldKeepChasing;
    }
    bool QuestionSearchRequested()
    {
       
        return _memory.SearchRequested;
    }

    bool QuestionIsSearching()
    {
 
        return _memory.IsSearching;
    }


}
