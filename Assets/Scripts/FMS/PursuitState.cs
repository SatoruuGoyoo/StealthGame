using UnityEngine;

public class PursuitState : EnemyState
{
    public PursuitState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Pursuit State");
    }

    public override void Update()
    {
        if (!model.DetectingEntity)
        {
            controller.ChangeState(new IdleState(controller));
            return;
        }

        Vector3 playerPos = model.CheckTarget().position;
        controller.MoveTo(playerPos);
    }
}
