using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel _model;
    private LineOfSight _los;

    [Header("Waypoints")]
    public Transform[] waypoints;
    public float moveSpeed = 2f;

    private EnemyState currentState;

    private void Awake()
    {
        _model = GetComponent<EnemyModel>();
        _los = new LineOfSight();
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        currentState?.OnUpdate();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public EnemyModel Model => _model;

    public void MoveTo(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public bool CanSeeTarget()
    {
        return _los.LoS(_model.transform, _model.CheckTarget(), _model.range, _model.angle, _model.obstacleMask);
    }
}