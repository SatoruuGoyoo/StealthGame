using System.Collections;
using TMPro;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance;

   
    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI messageText;

    void Awake()
    {
        
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowPauseText()
    {
        if (pauseText != null)
            pauseText.gameObject.SetActive(true);  
    }

    public void HidePauseText()
    {
        if (pauseText != null)
            pauseText.gameObject.SetActive(false);  
    }
    public void ShowMessage(string message, float duration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(message, duration));
    }

    IEnumerator ShowMessageRoutine(string message, float duration)
    {
        messageText.text = message;
        messageText.enabled = true;
        yield return new WaitForSeconds(duration);
        messageText.enabled = false;
    }
}
