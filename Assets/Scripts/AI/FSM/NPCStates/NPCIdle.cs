using UnityEngine;

public class NPCIdle<T> : NPCBase<T>
{
    public override void Enter()
    {
        base.Enter();
        _move.Move(Vector3.zero);
    }
}
