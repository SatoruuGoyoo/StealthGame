using System.Threading;
using UnityEngine;

public class IdleState : EnemyState
{
    private float idleTime = 3f;
    private float timer = 0f;

    public IdleState(EnemyController controller) : base(controller){}

    public override void Enter()
    {
        Debug.Log("Entering Idle State");
        timer = 0f;
    }

    public override void Update()
    {
        if (model.DetectingEntity)
        {
            controller.ChangeState(new PursuitState(controller));
            return;
        }

        timer += Time.deltaTime;

        if (timer >= idleTime)
        {
            controller.ChangeState(new PatrolState(controller));
        }
    }
        // Add any idle behavior here, such as playing an idle animation
        // or waiting for a certain amount of time before transitioning to the next state.
}
