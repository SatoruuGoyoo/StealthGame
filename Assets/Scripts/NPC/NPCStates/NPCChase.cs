using UnityEngine;

public class NPCChase<T> : NPCBase<T> 
{
    Transform _target;
    public NPCChase(Transform target)
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
