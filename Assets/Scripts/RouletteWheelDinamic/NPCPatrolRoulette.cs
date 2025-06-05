using UnityEngine;

public class NPCPatrolRoulette<T> : StatePathfinding<T>
{
    private PatrolPointSelector _selector;

    public NPCPatrolRoulette(Transform entity, IMove move, Animator anim, PatrolPointSelector selector, float distanceToPoint = 0.2f)
        : base(entity, move, anim, null, distanceToPoint)
    {
        _move = move;
        _anim = anim;
        _selector = selector;
    }

    public override void Initialize(params object[] p)
    {
        _move = (IMove)p[0];
        _selector = (PatrolPointSelector)p[1];
    }

    protected override void OnStartPath()
    {
        Debug.Log("[PATROL] Entrando a patrulla");

        _selector.UpdateWeights(_entity.position);
        var nextPoint = _selector.GetNextPatrolPoint();
        
        if (nextPoint == null)
        {
            Debug.LogWarning("[PATROL] No hay punto válido");
            return;
        }

        _target = nextPoint;
        Debug.Log($"[PATROL] Nuevo destino: {_target.position}");

        SetPathAStarPlusVector();
        _anim.SetFloat("Vel", 1);
    }

    protected override void OnFinishPath()
    {
        base.OnFinishPath();
        OnStartPath(); // Reinicia nuevo punto automáticamente
    }
}
