using UnityEngine;

public class NPCAttack<T> : NPCBase<T>
{
    public override void Enter()
    {
        base.Enter();
        _attack.Attack();
    }
}
