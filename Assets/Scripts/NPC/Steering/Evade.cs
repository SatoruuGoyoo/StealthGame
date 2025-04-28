using UnityEngine;

// Evade Steering Behavior
// Evade is a steering behavior that allows an entity to predict the future position of a target and move away from it.
public class Evade : Pursuit
{
    public Evade(Transform self, Rigidbody target) : base(self, target)
    {
    }
    public Evade(Transform self, Rigidbody target, float errorRange = 0, float timePrediction = 0) : base(self, target, errorRange, timePrediction)
    {
    }
    public Evade(Transform self, Rigidbody target, float errorRange = 0) : base(self, target, errorRange)
    {
    }
    public override Vector3 GetDir()
    {
        return -base.GetDir();
    }
}
