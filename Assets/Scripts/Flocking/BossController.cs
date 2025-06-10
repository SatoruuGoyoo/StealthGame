using UnityEngine;

public class BossController : NPCController
{
    private bool _alreadyAlerted = false;
    private LeaderBehaviour _leaderBehaviour;
    private ObstacleAvoidance _avoidance;

    protected override void Awake()
    {
        base.Awake();
        _leaderBehaviour = GetComponent<LeaderBehaviour>();
        _avoidance = GetComponent<ObstacleAvoidance>();
    }

    protected override void Update()
    {
        base.Update();

        if (!_alreadyAlerted && _los != null && target != null && _los.LOS(target.transform))
        {
            BossAlertManager.Instance.Alert(transform);
            _alreadyAlerted = true;

            if (_leaderBehaviour != null)
                _leaderBehaviour.Leader = target.transform;
        }


        // Movimiento de persecución del Boss
        if (_alreadyAlerted && _leaderBehaviour != null)
        {
            Vector3 dir = _leaderBehaviour.GetDir();

            if (_avoidance != null)
                dir = _avoidance.GetDir(dir);

            if (dir.sqrMagnitude > 0.01f)
            {
                _model.Move(dir.normalized);
                _model.LookDir(dir.normalized);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DefeatHandler.Instance.TriggerDefeat();
        }
    }
}
