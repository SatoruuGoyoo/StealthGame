using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main UI Elements")]
    public GameObject logo;
    public GameObject playButton;
    public GameObject controlsButton;
    public GameObject exitButton;

    [Header("Controls Screen Elements")]
    public GameObject controlsPanel;
    public GameObject returnButton;

    [Header("Codec Screen Elements")]
    public GameObject codecPanel;        
    public GameObject goToMissionButton; 

    [Header("Fade Transition")]
    public Image fadeImage;              
    public float fadeDuration = 1.5f;

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        controlsPanel.SetActive(false);
        returnButton.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeIn());
    }

    public void ShowControls()
    {
        logo.SetActive(false);
        playButton.SetActive(false);
        controlsButton.SetActive(false);
        exitButton.SetActive(false);

        controlsPanel.SetActive(true);
        returnButton.SetActive(true);
    }

    public void ReturnToMenu()
    {
        logo.SetActive(true);
        playButton.SetActive(true);
        controlsButton.SetActive(true);
        exitButton.SetActive(true);

        controlsPanel.SetActive(false);
        returnButton.SetActive(false);
    }

    public void OnPlayPressed()
    {
        logo.SetActive(false);
        playButton.SetActive(false);
        controlsButton.SetActive(false);
        exitButton.SetActive(false);

        codecPanel.SetActive(true);
        goToMissionButton.SetActive(true);
    }

    public void OnGoToMission()
    {
        StartCoroutine(FadeAndLoadScene("Level1")); 
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado.");
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
