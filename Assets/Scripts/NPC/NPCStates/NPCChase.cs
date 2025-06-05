using UnityEngine;

public class NPCChase<T> : StatePathfinding<T>
{
    private Transform _realTarget;
    private Transform _fakeTarget;

    private Vector3 _lastTargetPos;
    private float _repathThreshold = 0.5f;
    private float _repathTimer = 0f;
    private float _repathInterval = 0.5f;

    private ILook _look;

    public NPCChase(Transform entity, IMove move, ILook look, Animator anim, Transform target)
        : base(entity, move, anim, null)
    {
        _look = look;
        _realTarget = target;

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

        Vector3Int roundedPos = Vector3Int.RoundToInt(_realTarget.position);
        float distToLast = Vector3.Distance(_realTarget.position, _lastTargetPos);

        if (_repathTimer >= _repathInterval)
        {
            distToLast = Vector3.Distance(_realTarget.position, _lastTargetPos);
            float distToFakeTarget = Vector3.Distance(_entity.position, _fakeTarget.position);

            // Repath solo si el jugador se alejó lo suficiente O si ya estoy cerca del último destino
            if (distToLast > _repathThreshold || distToFakeTarget < 1.2f)
            {
                _lastTargetPos = _realTarget.position;
                _fakeTarget.position = Vector3Int.RoundToInt(_lastTargetPos);
                SetPathAStarPlusVector();
                _repathTimer = 0f;
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
        if (_fakeTarget != null)
            GameObject.Destroy(_fakeTarget.gameObject);
    }
}
