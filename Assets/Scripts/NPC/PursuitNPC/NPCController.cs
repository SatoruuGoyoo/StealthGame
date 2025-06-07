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

    private NPCSearch<StateEnum> _searchState;


    bool previousLOSState = false;

    public StateEnum CurrentStateEnum { get; protected set; }

    private void Awake()
    {
        _model = GetComponent<NPCModel>();
        _memory = _model.Memory;
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
        _searchState = new NPCSearch<StateEnum>(_model.transform, _model, anim, _memory);

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
        stateList.Add(_searchState);

        // Create Transitions
        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Attack, attack);
        idle.AddTransition(StateEnum.Patrol, patrol);

        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.Patrol, patrol);

        search.AddTransition(StateEnum.Chase, chase);
        search.AddTransition(StateEnum.Patrol, patrol);
        search.AddTransition(StateEnum.Search, search);


        //Chase
        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.Patrol, patrol);
        chase.AddTransition(StateEnum.Search, search);

        //Patrol
        patrol.AddTransition(StateEnum.Idle, idle);
        patrol.AddTransition(StateEnum.Chase, chase);
        patrol.AddTransition(StateEnum.Attack, attack);
        patrol.AddTransition(StateEnum.Patrol, patrol);



        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(_model, look, _model);
        }



        _fsm.SetInit(idle);
    }

    void InitializedTree()
    {
        var patrol = new ActionNode(() => 
        {
            if (_fsm.CurrentStateEnum != StateEnum.Patrol)
            {
                Debug.Log("TRANSICIONANDO A PATRULLA");
                _fsm.Transition(StateEnum.Patrol);
            }
        });
        var search = new ActionNode(() => 
        {
            Debug.Log("TRANSICIONANDO A SEARCH DESDE ÁRBOL");

            var points = SearchHelper.GetWeightedSearchPoints(transform.position, 4, 4f);

            _memory.SearchPoints = points;
            _memory.IsSearching = true;
            _memory.SearchRequested = true;

            _searchState.SetSearchPoints(points); // Asegurate de que _searchState esté accesible

            _fsm.Transition(StateEnum.Search);
        });

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
            Debug.Log("Cambio de visión detectado. Ve al jugador: " + currentLOS);
            OnTargetInView?.Invoke(currentLOS);

            if (!currentLOS)
            {
                Debug.Log("PERDIÓ DE VISTA AL JUGADOR → solicitar búsqueda");
                _memory.SearchRequested = true;
            }
            else
            {
                Debug.Log("VOLVIÓ A VER AL JUGADOR");
            }
        }

        previousLOSState = currentLOS;

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

    private void OnDrawGizmos()
    {
        if (_memory == null || _memory.SearchPoints == null || _memory.SearchPoints.Count == 0)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < _memory.SearchPoints.Count; i++)
        {
            Vector3 point = _memory.SearchPoints[i];
            Gizmos.DrawSphere(point + Vector3.up * 0.2f, 0.2f);

            if (i < _memory.SearchPoints.Count - 1)
            {
                Gizmos.DrawLine(point + Vector3.up * 0.2f, _memory.SearchPoints[i + 1] + Vector3.up * 0.2f);
            }
        }
    }

}
