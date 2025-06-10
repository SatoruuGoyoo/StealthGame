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

    private LeaderBehaviour _leader;

    private const float _graceTime = 5f; // 🕒 Tiempo de memoria visual
    private float _lastSeenTime = -Mathf.Infinity;

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

        if (_flocking != null && BossAlertManager.Instance.BossTransform != null)
        {
            _leader = BossAlertManager.Instance.BossTransform.GetComponent<LeaderBehaviour>();
            if (_leader != null)
            {
                _leader.IsActive = true;
                _flocking.SetLeader(_leader);
            }
        }

        if (!CanSeeBoss())
        {
            _isFlocking = false;
            SetPathAStarPlusVector(_self.position, _lastBossPos);
        }
        else
        {
            _lastSeenTime = Time.time;
            _isFlocking = true;
        }
    }

    public override void Execute()
    {
        Transform boss = BossAlertManager.Instance.BossTransform;
        if (boss == null) return;
        if (_flocking == null)
        {
            Debug.LogError($"{_self.name} → FlockingManager no asignado.");
            return;
        }

        bool seesBossNow = CanSeeBoss();

        // ✅ Memoria visual: si lo vio recientemente, aún lo "ve"
        bool shouldFlock = seesBossNow || Time.time - _lastSeenTime <= _graceTime;

        if (seesBossNow)
            _lastSeenTime = Time.time;

        if (shouldFlock)
        {
            _isFlocking = true;

            Vector3 dir = _flocking.GetDir();

            if (dir.sqrMagnitude < 0.01f)
            {
                Debug.Log($"{_self.name} → Dir de flocking nulo, usando A* como fallback.");
                _isFlocking = false;
                SetPathAStarPlusVector(_self.position, boss.position);
                base.Execute();
                return;
            }

            Debug.DrawLine(_self.position, _self.position + dir.normalized * 2f, Color.cyan);

            if (_avoidance != null)
                dir = _avoidance.GetDir(dir);

            _move.Move(dir.normalized);
            _look.LookDir(dir.normalized);
        }
        else
        {
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

            base.Execute(); // Pathfinding fallback
        }
    }

    public override void Sleep()
    {
        base.Sleep();

        if (_leader != null)
            _leader.IsActive = false;

        Debug.Log($"{_self.name} → EXIT: NPCFollowBoss");
    }

    private bool CanSeeBoss()
    {
        return _los != null &&
               BossAlertManager.Instance.BossTransform != null &&
               _los.LOS(BossAlertManager.Instance.BossTransform);
    }
}
