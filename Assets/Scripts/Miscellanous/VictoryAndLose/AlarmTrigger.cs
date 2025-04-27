using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{

    public GameObject[] alarmLights;
    public DefeatHandler defeatHandler;
    public bool alarmActivated = false;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (!alarmActivated && other.CompareTag("NPC2"))
        {
            alarmActivated = true;

            if (defeatHandler != null)
                defeatHandler.TriggerDefeat(); 

            foreach (var lightObj in alarmLights)
            {
                if (lightObj != null)
                    lightObj.SetActive(true);
            }
            AudioManager.instance.PlaySound("alarm");
            AudioManager.instance.StopSound("music");
            Destroy(player);

        }
    }
}
