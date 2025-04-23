using UnityEngine;

public class NPCPatrol<T> : NPCSBase<T>
{
    private Transform[] waypoints;
    private int index = 0;
    private int direction = 1;

    public NPCPatrol(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }

    public override void Enter()
    {
        base.Enter();
        index = 0;
        direction = 1;
    }

    public override void Execute()
    {
        if (_look.CanSeeTarget())
        {
            StateMachine.Transition((T)(object)StateEnum.Chase);
            return;
        }
        Vector3 targetPos = waypoints[index].position;
        Vector3 dir = (targetPos - _move.Position).normalized;

        _move.Move(dir);
        _look.LookDir(dir);

        if (Vector3.Distance(_move.Position, targetPos) < 0.1f)
        {
            index += direction;
            if (index >= waypoints.Length || index< 0)
            {
            direction *= -1;
            index += direction;
            }
        }

    
    }
public override void Exit() 
{ 

}

}
