using UnityEngine;

public class PursuitState : EnemyState
{
    public PursuitState(EnemyController controller) : base(controller) { }

    public override void OnUpdate()
    {
        if (controller.CanSeeTarget())
        {
            Vector3 targetPos = controller.Model.CheckTarget().position;
            controller.MoveTo(targetPos);
        }
        else
        {
            controller.ChangeState(new IdleState(controller));
        }
    }
}
