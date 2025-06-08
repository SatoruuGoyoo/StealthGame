using UnityEngine;

public class BossAlertManager : MonoBehaviour
{
    public static BossAlertManager Instance { get; private set; }

    public bool IsBossAlerted { get; private set; } = false;
    public Transform BossTransform { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Alert(Transform bossTransform)
    {
        IsBossAlerted = true;
        BossTransform = bossTransform;
    }

    public void ResetAlert()
    {
        IsBossAlerted = false;
    }

}
