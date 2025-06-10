// Enemy NPC Controller
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("NPC Waypoints")]
    public List<Transform> _patrolPoints;

    [Header("NPC Common Settings")]
    public Rigidbody target;

    FSM<StateEnum> _fsm;
    protected LineOfSightMono _los;
    protected NPCModel _model;
    protected ITreeNode _root;
    ISteering _steering;

    public event System.Action<bool> OnTargetInView;

    private NPCMemory _memory = new NPCMemory();
    private NPCSearch<StateEnum> _searchState;

    bool previousLOSState = false;
    public StateEnum CurrentStateEnum { get; protected set; }

    protected virtual void Awake()
    {
        _model = GetComponent<NPCModel>();
        _los = GetComponent<LineOfSightMono>();
        _steering = GetComponent<ISteering>();
    }

    void Start()
    {
        InitializedFSM();
        InitializedTree();
    }

    protected virtual void Update()
    {
        _fsm.OnExecute();
        _root.Execute();
    }

    private void FixedUpdate()
    {
        _fsm.OnFixExecute();
    }

    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var look = GetComponent<ILook>();
        var anim = GetComponent<Animator>();

        var idle = new NPCIdle<StateEnum>();
        var attack = new NPCAttack<StateEnum>();
        var patrol = new NPCPatrol<StateEnum>(_model.transform, _model, look, anim, _patrolPoints);
        var chase = new NPCChase<StateEnum>(_model.transform, _model, look, anim, target.transform);
        var search = new NPCSearch<StateEnum>(_model.transform, _model, anim, _memory);
        _searchState = search;

        NPCFollowBoss<StateEnum> followBoss = null;
        if (!(this is BossController))
        {
            followBoss = new NPCFollowBoss<StateEnum>(_model.transform, _model, look, anim);
        }

        var stateList = new List<IState<StateEnum>> { idle, attack, patrol, chase, search };
        if (followBoss != null) stateList.Add(followBoss);

        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Attack, attack);
        idle.AddTransition(StateEnum.Patrol, patrol);
        if (followBoss != null) idle.AddTransition(StateEnum.FollowBoss, followBoss);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.Patrol, patrol);

        search.AddTransition(StateEnum.Chase, chase);
        search.AddTransition(StateEnum.Patrol, patrol);

        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.Patrol, patrol);
        chase.AddTransition(StateEnum.Search, search);

        patrol.AddTransition(StateEnum.Idle, idle);
        patrol.AddTransition(StateEnum.Chase, chase);
        patrol.AddTransition(StateEnum.Attack, attack);
        if (followBoss != null) patrol.AddTransition(StateEnum.FollowBoss, followBoss);

        if (followBoss != null)
        {
            followBoss.AddTransition(StateEnum.Patrol, patrol);
            followBoss.AddTransition(StateEnum.Attack, attack);
        }

        foreach (var state in stateList)
        {
            state.Initialize(_model, look, _model);
        }

        _fsm.SetInit(idle);
    }

    protected virtual void InitializedTree()
    {
        var patrol = new ActionNode(() => _fsm.Transition(StateEnum.Patrol));
        var search = new ActionNode(() => _fsm.Transition(StateEnum.Search));
        var chase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
        var attack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));
        var followBoss = new ActionNode(() => _fsm.Transition(StateEnum.FollowBoss));

        var qAttack = new QuestionNode(QuestionCanAttack, attack, chase);
        var qChase = new QuestionNode(QuestionCanSeePlayer, qAttack, search);
        var qSearchRequest = new QuestionNode(QuestionSearchRequested, search, patrol);
        var qIsSearching = new QuestionNode(QuestionIsSearching, search, qSearchRequest);

      
        var qIfBossAlerted = new QuestionNode(QuestionIsBossAlerted, followBoss, qIsSearching);
        var qRoot = new QuestionNode(QuestionCanSeePlayer, qAttack, qIfBossAlerted);

        _root = qRoot;
    }

    protected bool QuestionCanAttack()
    {
        return Vector3.Distance(_model.Position, target.position) <= _model.attackRange;
    }

    protected bool QuestionIsBossAlerted()
    {
        return BossAlertManager.Instance.IsBossAlerted;
    }

    protected bool QuestionCanSeePlayer()
    {
        if (target == null) return false;

        bool currentLOS = _los.LOS(target.transform);

        if (currentLOS != previousLOSState)
        {
            OnTargetInView?.Invoke(currentLOS);
            previousLOSState = currentLOS;
            if (!currentLOS) _memory.SearchRequested = true;
        }

        if (currentLOS)
            _memory.UpdateLastSeen();

        return _memory.ShouldKeepChasing;
    }

    protected bool QuestionSearchRequested()
    {
        return _memory.SearchRequested;
    }

    protected bool QuestionIsSearching()
    {
        return _memory.IsSearching;
    }

    private void OnDrawGizmos()
    {
        if (_searchState == null) return;

        var points = _searchState.GetSearchPoints();
        if (points == null || points.Count == 0) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawSphere(points[i] + Vector3.up * 0.2f, 0.2f);
            if (i < points.Count - 1)
            {
                Gizmos.DrawLine(points[i] + Vector3.up * 0.2f, points[i + 1] + Vector3.up * 0.2f);
            }
        }
    }
}
