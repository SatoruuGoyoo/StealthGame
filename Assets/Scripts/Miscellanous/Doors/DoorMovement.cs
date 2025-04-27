using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3f, 0); 
    public float openSpeed = 2f;
    private bool opening = false;
    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + openOffset;
    }

    public void OpenDoor()
    {
        opening = true;
    }

    void Update()
    {
        if (opening)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, openSpeed * Time.deltaTime);
        }
    }
}
