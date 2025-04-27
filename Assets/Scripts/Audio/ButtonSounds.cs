using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    [Header("Sonidos")]
    public string clickSound = "UIButtonClick";
    public string hoverSound = "UIButtonHover";

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(hoverSound))
            AudioManager.instance?.PlaySound(hoverSound);
    }

    void PlayClickSound()
    {
        if (!string.IsNullOrEmpty(clickSound))
            AudioManager.instance?.PlaySound(clickSound);
    }
}
