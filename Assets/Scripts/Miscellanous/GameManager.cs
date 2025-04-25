using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Loading,
        InGame,
        Paused,
        GameOver,
        Victory
    }
    public static GameManager Instance;

    private FSM<GameState> _fsm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
