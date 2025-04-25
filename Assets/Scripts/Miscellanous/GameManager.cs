using Unity.Loading;
using UnityEditor;
using UnityEngine;
using static GameManager;

public class GameManager : MonoBehaviour
{
   
    public static GameManager Instance;

    private FSM<GameState> _fsm;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        
        var mainMenu = new MainMenuState();
        var loading = new LoadingState();
        var inGame = new InGameState();
        var paused = new PausedState();
       

       
        mainMenu.AddTransition(GameState.Loading, loading);
        loading.AddTransition(GameState.InGame, inGame);
        inGame.AddTransition(GameState.Paused, paused);
        paused.AddTransition(GameState.InGame, inGame);
     ;

        
        _fsm = new FSM<GameState>(mainMenu);
    }
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }
}
