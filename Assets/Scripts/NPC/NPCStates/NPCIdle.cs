using UnityEngine;

public class NPCIdle<T> : NPCBase<T>
{
    float _timer;
    float _waitTime = 1f;

    public override void Enter()
    {
        base.Enter();
        _timer = 0f;
        
    }

    public override void Execute()
    {
        base.Execute();

        _timer += Time.deltaTime;

        if (_timer >= _waitTime)
        {
           
            StateMachine.Transition((T)(object)StateEnum.Patrol);
        }

    }
}
