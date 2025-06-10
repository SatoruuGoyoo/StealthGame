using System.Collections.Generic;
using UnityEngine;

public class FollowLeaderBehaviour : FlockingBaseBehaviour, IFlocking
{
    private Transform _leader;

    // Set the leader transform that the boid should follow
    public void SetLeader(Transform leader)
    {
        _leader = leader;
    }

    // Calculates direction toward the leader (if valid), scaled by multiplier
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_leader == null || self == null)
            return Vector3.zero;

        Vector3 toLeader = _leader.position - self.Position;
        float dist = toLeader.magnitude;

        // If too close to the leader, avoid applying force to prevent clustering
        if (dist < 0.5f)
            return Vector3.zero;

        return toLeader.normalized * multiplier;
    }
}
