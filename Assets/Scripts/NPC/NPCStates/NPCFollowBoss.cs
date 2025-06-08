using UnityEngine;

public class NPCFollowBoss<T> : StatePathfinding<T>
{
    private new IMove _move;
    private new ObstacleAvoidance _avoidance;
    private ILook _look;
    private FlockingManager _flocking;
    private LineOfSightMono _los;
    private Transform _self;

    private Vector3 _lastBossPos;
    private float _repathThreshold = 1.5f;

    public NPCFollowBoss(Transform self, IMove move, ILook look, Animator anim)
        : base(self, move, anim)
    {
        _self = self;
        _move = move;
        _look = look;
    }

    public override void Enter()
    {
        base.Enter();

        _flocking = _self.GetComponent<FlockingManager>();
        _avoidance = _self.GetComponent<ObstacleAvoidance>();
        _los = _self.GetComponent<LineOfSightMono>();

        _lastBossPos = BossAlertManager.Instance.BossTransform != null
            ? BossAlertManager.Instance.BossTransform.position
            : _self.position;

        if (!CanSeeBoss())
        {
            SetPathAStarPlusVector(_self.position, _lastBossPos);
        }
    }

    public override void Execute()
    {
        Transform boss = BossAlertManager.Instance.BossTransform;
        if (boss == null) return;

        if (CanSeeBoss())
        {
            float dist = Vector3.Distance(_self.position, boss.position);
            if (dist > 3f)
            {
                // Demasiado lejos para flocking → seguir con pathfinding
                SetPathAStarPlusVector(_self.position, boss.position);
                base.Execute();
                return;
            }

            // Flocking activo
            Vector3 dir = _flocking.GetDir();
            if (_avoidance != null)
                dir = _avoidance.GetDir(dir);

            _move.Move(dir.normalized);
            _look.LookDir(dir.normalized);
        }
        else
        {
            // Si el boss se movió mucho, recalcular el path
            float distToLast = Vector3.Distance(_lastBossPos, boss.position);
            if (distToLast > _repathThreshold)
            {
                _lastBossPos = boss.position;
                SetPathAStarPlusVector(_self.position, _lastBossPos);
            }

            base.Execute(); // sigue la ruta
        }
    }

    private bool CanSeeBoss()
    {
        return _los != null &&
               BossAlertManager.Instance.BossTransform != null &&
               _los.LOS(BossAlertManager.Instance.BossTransform);
    }
}
