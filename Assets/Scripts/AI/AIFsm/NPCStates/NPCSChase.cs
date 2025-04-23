using UnityEngine;

public class NPCSChase<T> : NPCSBase<T>
{
    Transform _target;

    public NPCSChase(Transform target)
    {
        _target = target;
    }

    public override void Execute()
    {
        base.Execute();
        
        var dir = _target.transform.position - _move.Position;

        _move.Move(dir.normalized);
        _look.LookDir(dir.normalized);
        
    }
}
