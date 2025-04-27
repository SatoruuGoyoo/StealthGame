using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject player; 
    public GameObject VictoryUI;

    private void Start()
    {
        VictoryUI.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
     
        if (other.gameObject == player)
        {
           VictoryUI.SetActive(true);
            GameManager.Instance.ChangeState(GameState.Victory); 
        }
    }
}
