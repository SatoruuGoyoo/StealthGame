using UnityEngine;

// NPCIdle class for the NPC state machine
// NPC in idle state
public class NPCIdle<T> : NPCBase<T>
{
   private float _idleTimer = 0f;
    private const float _idleDuration = 3f; // Duration for idle state

    public override void Enter()
    {
        base.Enter();
        _idleTimer = 0f;
    }

    public override void Execute()
    {
        base.Execute();
        _move.Move(Vector3.zero);
        _idleTimer += Time.deltaTime;

    }



}
