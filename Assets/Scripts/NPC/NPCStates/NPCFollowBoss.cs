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
    private bool _isFlocking = false;

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
        Debug.Log($"ENTER: {_self.name} NPCFollowBoss");

        _flocking = _self.GetComponent<FlockingManager>();
        _avoidance = _self.GetComponent<ObstacleAvoidance>();
        _los = _self.GetComponent<LineOfSightMono>();

        _lastBossPos = BossAlertManager.Instance.BossTransform != null
            ? BossAlertManager.Instance.BossTransform.position
            : _self.position;

        // Setear líder
        if (_flocking != null && BossAlertManager.Instance.BossTransform != null)
        {
            var leader = BossAlertManager.Instance.BossTransform.GetComponent<LeaderBehaviour>();
            if (leader != null)
                _flocking.SetLeader(leader);
        }

        if (!CanSeeBoss())
        {
            _isFlocking = false;
            SetPathAStarPlusVector(_self.position, _lastBossPos);
        }
        else
        {
            _isFlocking = true;
        }
    }

    public override void Execute()
    {
        base.Execute();

        Transform boss = BossAlertManager.Instance.BossTransform;
        if (boss == null) return;
        if (_flocking == null)
        {
            Debug.LogError($"{_self.name} → FlockingManager no asignado.");
            return;
        }

        if (CanSeeBoss())
        {
            float dist = Vector3.Distance(_self.position, boss.position);
            if (dist > 3f || !_flocking.HasNeighbors())
            {
                _isFlocking = false;
                SetPathAStarPlusVector(_self.position, boss.position);
                base.Execute();
                return;
            }

            Vector3 dir = _flocking.GetDir();

            if (dir.sqrMagnitude < 0.01f)
            {
                Debug.Log($"{_self.name} → Dir de flocking nulo, volviendo a A*");
                _isFlocking = false;
                SetPathAStarPlusVector(_self.position, boss.position);
                base.Execute();
                return;
            }

            _isFlocking = true;
            Debug.DrawLine(_self.position, _self.position + dir.normalized * 2f, Color.cyan);

            if (_avoidance != null)
                dir = _avoidance.GetDir(dir);

            _move.Move(dir.normalized);
            _look.LookDir(dir.normalized);
        }
        else
        {
            //  Perdió de vista
            if (_isFlocking)
            {
                _isFlocking = false;
                SetPathAStarPlusVector(_self.position, _lastBossPos);
            }
            else
            {
                float distToLast = Vector3.Distance(_lastBossPos, boss.position);
                if (distToLast > _repathThreshold)
                {
                    _lastBossPos = boss.position;
                    SetPathAStarPlusVector(_self.position, _lastBossPos);
                }
            }
        }
    }

    private bool CanSeeBoss()
    {
        return _los != null &&
               BossAlertManager.Instance.BossTransform != null &&
               _los.LOS(BossAlertManager.Instance.BossTransform);
    }
}
