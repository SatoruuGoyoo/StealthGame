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

    private static Transform CreateFakeTarget()
    {
        var go = new GameObject("FakeTarget_Chase");
        go.hideFlags = HideFlags.HideInHierarchy;
        return go.transform;
    }

    public NPCChase(Transform entity, IMove move, ILook look, Animator anim, Transform target)
        : base(entity, move, anim)
    {
        _look = look;
        _realTarget = target;
        _fakeTarget = CreateFakeTarget();
    }

    public override void Enter()
    {
        base.Enter();

        if (_fakeTarget == null)
        {
            _fakeTarget = CreateFakeTarget();
        }

        _repathTimer = 0f;
        _lastTargetPos = _realTarget.position;
        _fakeTarget.position = Vector3Int.RoundToInt(_lastTargetPos);

        SetPathAStarPlusVector(_move.Position, _fakeTarget.position);
    }

    public override void Execute()
    {
        base.Execute();

        _repathTimer += Time.deltaTime;

        float distToLast = Vector3.Distance(_realTarget.position, _lastTargetPos);
        float distToFakeTarget = Vector3.Distance(_move.Position, _fakeTarget.position);

        if (_repathTimer >= _repathInterval &&
            (distToLast > _repathThreshold || IsFinishPath || distToFakeTarget < 1.2f))
        {
            _lastTargetPos = _realTarget.position;
            _fakeTarget.position = Vector3Int.RoundToInt(_lastTargetPos);
            SetPathAStarPlusVector(_move.Position, _fakeTarget.position);
            _repathTimer = 0f;
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (_fakeTarget != null)
            GameObject.Destroy(_fakeTarget.gameObject);

        _fakeTarget = null;
    }
}
