using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [SerializeField] private Transform target; 
    [SerializeField] private Vector3 offset = new Vector3(5, 10, -5);
    [SerializeField] private bool lookAtTarget = true;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}

