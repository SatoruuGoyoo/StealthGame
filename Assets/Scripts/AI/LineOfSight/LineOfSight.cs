using UnityEngine;

public class LineOfSight
{

    // A: AI
    // B: Target (player)
    public bool CheckRange(Transform self, Transform target, float range)
    {
        Vector3 dir = target.position - self.position; // B-A
        float distance = dir.magnitude;
        return distance <= range; // Check if the distance is within range
    }

    public bool CheckAngle(Transform self, Transform target, float angle)
    {
        Vector3.Angle(self.forward, target.position - self.position); // A-B
    }
}
