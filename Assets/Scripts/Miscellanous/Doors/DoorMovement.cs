using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3f, 0);
    public float moveSpeed = 2f;
    public Collider triggerArea; 

    private Vector3 startPos;
    private Vector3 openPos;
    private bool opening = false;
    private bool closing = false;

    void Start()
    {
        startPos = transform.position;
        openPos = startPos + openOffset;
    }

    public void OpenDoor()
    {
        opening = true;
        closing = false;
    }

    void CloseDoor()
    {
        closing = true;
        opening = false;
    }

    void Update()
    {
        if (opening)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPos, moveSpeed * Time.deltaTime);
        }
        else if (closing)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggerArea != null && other == triggerArea)
        {
            CloseDoor();
        }
    }
}
