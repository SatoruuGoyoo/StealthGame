using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingState : State<GameState>
{
    public override void Enter()
    {
        Debug.Log("🕐 Cargando con fade...");
        GameManager.Instance.StartCoroutine(FadeAndLoadScene("Level1"));
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        var fade = MainMenu.Instance?.fadeImage;
        if (fade == null)
        {
            Debug.LogWarning("No se encontró el fadeImage. Cargando directo.");
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        fade.gameObject.SetActive(true);
        float t = 0f;
        float duration = MainMenu.Instance.fadeDuration;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / duration);
            fade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        GameManager.Instance.ChangeState(GameState.InGame);
        SceneManager.LoadScene(sceneName);
    }
}
