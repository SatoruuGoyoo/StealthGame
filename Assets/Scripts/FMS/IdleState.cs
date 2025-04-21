using System.Threading;
using UnityEngine;

public class IdleState : EnemyState
{
    private float timer = 2f;

    public IdleState(EnemyController controller) : base(controller) { }

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            controller.ChangeState(new PatrolState(controller));
        }
        else if (controller.CanSeeTarget())
        {
            controller.ChangeState(new PursuitState(controller));
        }
    }
}
