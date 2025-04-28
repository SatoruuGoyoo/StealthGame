using UnityEngine;

// NPCAttack class for the NPC state machine
// This class represents the attack state of an NPC.
public class NPCAttack<T> : NPCBase<T>
{
    public override void Enter()
    {
        base.Enter();
        _attack.Attack();
    }
}
