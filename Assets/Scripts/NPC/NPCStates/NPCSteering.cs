using UnityEngine;

// NPC Steering class for the NPC state machine
// This class represents the steering behavior of an NPC.
// It uses an ISteering interface to get the direction for movement.

public class NPCSteering<T> : NPCBase<T>
{
    ISteering _steering;
    public NPCSteering(ISteering steering)
    {
        _steering = steering;
    }
    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        _move.Move(dir.normalized);
    }
    public void ChangeSteering(ISteering newSteering)
    {
        _steering = newSteering;
    }
}
