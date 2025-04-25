using UnityEngine;

public class DefeatHandler : MonoBehaviour
{
    public bool Defeated;
    public GameObject DefeatUI;
    void Start()
    {
        DefeatUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Defeated)
        {
            DefeatUI.SetActive(true);
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }
}
