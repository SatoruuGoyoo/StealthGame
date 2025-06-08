using System.Collections.Generic;
using UnityEngine;

public class Alignment : FlockingBaseBehaviour
{
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < boids.Count; i++)
        {
            alignment += boids[i].Forward;
        }

        return alignment.normalized * multiplier;
    }
}
