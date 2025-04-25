using TMPro;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance;

   
    public TextMeshProUGUI pauseText;

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
}
