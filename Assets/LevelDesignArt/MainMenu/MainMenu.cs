using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


    public GameObject logo, playButton, controlsButton, exitButton;
    public GameObject controlsPanel, returnButton;
    public GameObject codecPanel, goToMissionButton;
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    public static MainMenu Instance;

    void Awake()
    {
        Instance = this;
    }
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
        //StartCoroutine(FadeAndLoadScene());
        GameManager.Instance.ChangeState(GameState.Loading);
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

    //IEnumerator FadeAndLoadScene()
    //{
    //    fadeImage.gameObject.SetActive(true);
    //    float t = 0;
    //    while (t < fadeDuration)
    //    {
    //        t += Time.deltaTime;
    //        float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
    //        fadeImage.color = new Color(0, 0, 0, alpha);
    //        yield return null;
    //    }
    //}
}
