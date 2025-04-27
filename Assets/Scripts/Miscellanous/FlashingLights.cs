using UnityEngine;

public class FlashingLights : MonoBehaviour
{
    public float flashSpeed = 2f;
    private Light alarmLight;

    void Start()
    {
        alarmLight = GetComponent<Light>();
    }

    void Update()
    {
        if (alarmLight != null)
            alarmLight.intensity = Mathf.PingPong(Time.unscaledTime * flashSpeed, 150f);
    }
}
