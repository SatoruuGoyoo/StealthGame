using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    public GameObject[] alarmLights; 
    public DefeatHandler defeatHandler;
    private bool alarmActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!alarmActivated && other.CompareTag("NPC2"))
        {
            alarmActivated = true;

            if (defeatHandler != null)
                defeatHandler.Defeated = true;

            foreach (var lightObj in alarmLights)
            {
                if (lightObj != null)
                    lightObj.SetActive(true);
            }
        }
        AudioManager.instance.PlaySound("alarm");
        AudioManager.instance.StopSound("music");
        Time.timeScale = 0f;
    }
}
