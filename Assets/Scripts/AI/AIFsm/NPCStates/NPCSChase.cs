using UnityEngine;

public class NPCSChase<T> : NPCSBase<T>
{
    private Transform _target;

    public NPCSChase(Transform target)
    {
        _target = target;
    }

    public override void Execute()
    {
        if(!_look.CanSeeTarget())
        {
            StateMachine.Transition((T)(object)StateEnum.Idle);
            return;
        }

        Vector3 dir = (_target.position - _move.Position).normalized;

        _move.Move(dir);
        _look.LookDir(dir);
        
    }
}
