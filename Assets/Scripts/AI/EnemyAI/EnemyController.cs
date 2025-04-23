using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform target;
    public Transform _initialZone;
    FSM<StateEnum> _fsm;
    private EnemyModel _model;
    private LineOfSightMono _los;
    ITreeNode _root;
    private bool _isInitialized = false;



    private void Start()
    {
        InitializeFSM();
        InitializeTree();
        _isInitialized = true;
    }

    void Update()
    {
        if (!_isInitialized) return;

        _fsm.OnExecute();
        _root?.Execute();

        //_fsm.OnExecute();
        //_root.Execute();
    }

    private void Awake()
    {
        _model = GetComponent<EnemyModel>();
        _los = GetComponent<LineOfSightMono>();

        //if (target == null)
        //{
        //    GameObject player = GameObject.FindWithTag("Player");
        //    if (player != null)
        //        target = player.transform;
        //}
    }

    void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();

        var look = GetComponent<ILook>();

        // States created
        var idle = new NPCSIdle<StateEnum>();
        var attack = new NPCSAttack<StateEnum>();
        var chase = new NPCSChase<StateEnum>(target);
        var goZone = new NPCPatrol<StateEnum>(waypoints);

        // Added to a LIST
        var stateList = new List<NPCSBase<StateEnum>>();
        stateList.Add(idle);
        stateList.Add(attack);
        stateList.Add(chase);
        stateList.Add(goZone);

        // Created Transitions
        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.GoZone, goZone);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        chase.AddTransition(StateEnum.GoZone, goZone);

        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.GoZone, goZone);

        goZone.AddTransition(StateEnum.Idle, idle);
        goZone.AddTransition(StateEnum.Chase, chase);
        goZone.AddTransition(StateEnum.Attack, attack);

        // Go trough List
        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(_model, look, _model);
        }

        _fsm.SetInit(idle);

    }

    void InitializeTree()
    {
        var idle = new ANode(() => _fsm.Transition(StateEnum.Idle));
        var attack = new ANode(() => _fsm.Transition(StateEnum.Attack));
        var chase = new ANode(() => _fsm.Transition(StateEnum.Chase));
        var goZone = new ANode(() => _fsm.Transition(StateEnum.GoZone));

        var qCanAttack = new QNode(QuestionCanAttack, attack , chase);
        var qCanGoZone = new QNode(QuestionCanGoZone, goZone, idle);
        var qCanSeeTarget = new QNode(QuestionCanSeeTarget, qCanAttack, qCanGoZone);

        _root = qCanSeeTarget;
    }
    bool QuestionCanAttack()
    {
        return Vector3.Distance(_model.Position, target.position) <= _model.attackRange;
        
    }
    bool QuestionCanSeeTarget()
    {
        if(target == null) return false;
        return _los.LOS(target);
    }
    bool QuestionCanGoZone()
    {
        return Vector3.Distance(_model.transform.position, _initialZone.position) > 0.25f;
    }


    //private void Update()
    //{
    //    var target = _model.CheckTarget();
    //    if (_los.LoS(_model.transform, _model.CheckTarget(), _model.range, _model.angle, _model.obstacleMask))
    //    {
    //        print("Target in range and angle, no obstacle");
    //        _model.DetectingEntity = true;
    //    }
    //    else
    //    {
    //        print("Target out of range or angle, or obstacle in the way");
    //        _model.DetectingEntity = false;
    //    }

    //}



}
