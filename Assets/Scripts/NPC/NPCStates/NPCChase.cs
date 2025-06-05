using UnityEngine;

public class NPCChase<T> : StatePathfinding<T>
{
    private Transform _realTarget;
    private Transform _fakeTarget;

    private Vector3 _lastTargetPos;
    private float _repathThreshold = 0.5f;
    private float _repathTimer = 0f;
    private float _repathInterval = 1f;

    private ILook _look;

    // Al definir:
    private float _predictionTime = 0.75f;
    private Rigidbody _playerRb;


    public NPCChase(Transform entity, IMove move, ILook look, Animator anim, Transform target)
    : base(entity, move, anim, null)
    {
        _look = look;
        _realTarget = target;
        _playerRb = target.GetComponent<Rigidbody>();

        _fakeTarget = new GameObject("FakeTarget_Chase").transform;
        _fakeTarget.hideFlags = HideFlags.HideInHierarchy;
        base._target = _fakeTarget;
    }


    public override void Enter()
    {
        base.Enter();

        _repathTimer = 0f;

        if (_fakeTarget == null)
        {
            _fakeTarget = new GameObject("FakeTarget_Chase").transform;
            _fakeTarget.hideFlags = HideFlags.HideInHierarchy;
            base._target = _fakeTarget;
        }

        _lastTargetPos = Vector3Int.RoundToInt(_realTarget.position);
        _fakeTarget.position = _lastTargetPos;
        SetPathAStarPlusVector();
    }

    public override void Execute()
    {
        base.Execute();

        _repathTimer += Time.deltaTime;

        float distToLast = Vector3.Distance(_realTarget.position, _lastTargetPos);
        float distToFakeTarget = Vector3.Distance(_entity.position, _fakeTarget.position);

        // No recalcules si no terminaste el path actual, salvo que el jugador se haya alejado mucho
        if (_repathTimer >= _repathInterval &&
    (       IsFinishPath || distToLast > 2f || distToFakeTarget > 4f))

        {
            _lastTargetPos = PredictPlayerPosition();

            _fakeTarget.position = Vector3Int.RoundToInt(_lastTargetPos);
            SetPathAStarPlusVector();
            _repathTimer = 0f;
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (_fakeTarget != null)
            GameObject.Destroy(_fakeTarget.gameObject);
    }

    private Vector3 PredictPlayerPosition()
    {
        if (_playerRb == null)
            return _realTarget.position;

        Vector3 velocity = _playerRb.linearVelocity;
        Vector3 predicted = _realTarget.position + velocity * _predictionTime;
        return predicted;
    }

}
