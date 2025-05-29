using UnityEngine;

// NPCIdle class for the NPC state machine
// NPC in idle state
public class NPCIdle<T> : NPCBase<T>
{
   

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Execute()
    {
        base.Execute();
        _move.Move(Vector3.zero);
    }
}
