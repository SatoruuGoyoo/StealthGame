using UnityEngine;

public class NPCSIdle<T> : NPCSBase<T>
{
    private float timer;
    private float idleTime = 3f;

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        _move.Move(Vector3.zero);

        // Trigger visuales, anims
    }

    private override void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            StateMachine.Transition((T)(object)StateEnum.Patrol);
        }

        if(_look.CanSeeTarget())
        {
            StateMachine.Transition((T)(object)StateEnum.Chase);
        }
    }
 
    public override void FixExecute()
    {
        // Do nothing
    }

    public override void Exit()
    {
        // Trigger visuales, anims
    }
}
