using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    public string keycardID = "MainKeycard"; 
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySound("pick");
            PlayerInventory.Instance.CollectKeycard(keycardID);
            collected = true;
            Destroy(gameObject); 
        }
    }
}
