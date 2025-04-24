using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Rigidbody target;
    public Transform zone;
    FSM<StateEnum> _fsm;
    NPCModel _model;
    LineOfSightMono _los;
    ITreeNode _root;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
