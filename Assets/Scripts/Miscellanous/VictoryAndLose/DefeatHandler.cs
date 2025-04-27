using UnityEngine;

public class DefeatHandler : MonoBehaviour
{
    public static DefeatHandler Instance;

    public bool Defeated;
    public GameObject DefeatUI;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DefeatUI.SetActive(false);
    }

    void Update()
    {
        if (Defeated)
        {
            DefeatUI.SetActive(true);
            GameManager.Instance.ChangeState(GameState.GameOver);
            //Time.timeScale = 0f; 
        }
    }

    public void TriggerDefeat()
    {
        Defeated = true;
    }
}
