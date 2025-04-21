using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private EnemyModel _model;
    private LineOfSight _los;
    private EnemyState currentState;

    public EnemyModel Model => _model;
    public Transform[] Waypoints => waypoints;

    private void Awake()
    {
        _los = new LineOfSight();
        _model = GetComponent<EnemyModel>();
    }

    private void Start()
    {
        ChageState(new PatrolState(this));
    }

    private void Update()
    {
        var target = _model.CheckTarget();
        if (_los.LoS(_model.transform, target, _model.range, _model.angle, _model.obstacleMask))
        {
            print("Target in range and angle, no obstacle");
            _model.DetectingEntity = true;
        }
        else
        {
            print("Target out of range or angle, or obstacle in the way");
            _model.DetectingEntity = false;
        }

        currentState?.Update();

    }

    public void ChageState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void MoveTo(Vector3 destination)
    {
        Vector3 dir = (destination - transform.position).normalized;
        float speed = 2f;
        transform.position += dir * speed * Time.deltaTime;
    }



}
