using UnityEngine;

public class NPCChase<T> : StatePathfinding<T>
{
    private Transform _target;
    private Vector3 _lastTargetPos;

    private float _repathThreshold = 0.5f;
    private float _repathInterval = 0.25f;
    private float _repathTimer = 0f;

    public NPCChase(Transform entity, IMove move, ILook look, Animator anim, Transform target)
        : base(entity, move, anim)
    {
        _target = target;
    }

    public override void Enter()
    {
        base.Enter();
        _lastTargetPos = _target.position;
        SetPathTo(_lastTargetPos);
        _repathTimer = 0f;
    }

    public override void Execute()
    {
        base.Execute();
        _repathTimer += Time.deltaTime;

        float distMoved = Vector3.Distance(_target.position, _lastTargetPos);
        float distToLast = Vector3.Distance(_entity.position, _lastTargetPos);

        if (_repathTimer >= _repathInterval && IsFinishPath == false &&
           (distMoved > _repathThreshold || distToLast < 1.2f))

        {
            _lastTargetPos = _target.position;
            SetPathTo(_lastTargetPos);
            _repathTimer = 0f;
        }
    }
}
