using UnityEngine;

public class KeycardPanel : MonoBehaviour
{
    public string requiredKeycardID = "MainKeycard";
    public GameObject doorToOpen;


    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerInventory.Instance.HasKeycard(requiredKeycardID))
            {
             
                doorToOpen.GetComponent<DoorMovement>().OpenDoor();
            }
            else
            {
                PauseUI.Instance.ShowMessage("Requiere una tarjeta", 4f);
               
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            PauseUI.Instance.ShowMessage("Presione E para interactuar", 4f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}
