using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour, ILook
{
    //M: Model
    //V: View
    //C: Controller
    [SerializeField]
   
    Rigidbody _rb;
    public float speedRot = 10;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
   

    public void LookDir(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRot);
    }

  
}
