using UnityEngine;

public class StatePatrolWithRoulette<T> : StatePathfinding<T>
{
    private PatrolPointSelector _selector;

    public StatePatrolWithRoulette(Transform entity, IMove move, Animator anim, PatrolPointSelector selector, float distanceToPoint = 0.2f)
        : base(entity, move, anim, null, distanceToPoint)
    {
        _selector = selector;
    }

    protected override void OnStartPath()
    {
        _selector.UpdateWeights(_entity.position);
        var targetPoint = _selector.GetNextPatrolPoint();
        if (targetPoint == null) return;

        _target = targetPoint;
        SetPathAStarPlusVector();
        _anim.SetFloat("Vel", 1);
    }

    protected override void OnFinishPath()
    {
        base.OnFinishPath();
        OnStartPath();
    }
}
