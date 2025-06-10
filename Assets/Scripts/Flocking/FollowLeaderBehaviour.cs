using System.Collections.Generic;
using UnityEngine;

public class FollowLeaderBehaviour : FlockingBaseBehaviour, IFlocking
{
    private Transform _leader;

    public void SetLeader(Transform leader)
    {
        _leader = leader;
    }

    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_leader == null || self == null)
            return Vector3.zero;

        Vector3 toLeader = _leader.position - self.Position;
        float dist = toLeader.magnitude;

        if (dist < 0.5f) // Están demasiado cerca
        {
            Debug.Log($"{self} → Muy cerca del líder, distancia: {dist}, no aplico fuerza");
            return Vector3.zero;
        }

        Vector3 dir = toLeader.normalized * multiplier;
        Debug.DrawLine(self.Position, _leader.position, Color.yellow);
        Debug.Log($"{self} → dir hacia líder: {dir}");
        return dir;
    }

}
