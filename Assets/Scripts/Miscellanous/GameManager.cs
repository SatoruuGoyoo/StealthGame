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

        InitFSM();
    }

    void Update() => _fsm?.OnExecute();
    void FixedUpdate() => _fsm?.OnFixExecute();

    public void ChangeState(GameState state)
    {
        _fsm?.Transition(state);
    }

    private void InitFSM()
    {
        _fsm = new FSM<GameState>();

        var main = new MainMenuState();
        var loading = new LoadingState();
        var game = new InGameState();
        var pause = new PausedState();
      

        main.AddTransition(GameState.Loading, loading);
        loading.AddTransition(GameState.InGame, game);
        game.AddTransition(GameState.Paused, pause);
    
        pause.AddTransition(GameState.InGame, game);
        ;

        main.Initialize();
        loading.Initialize();
        game.Initialize();
        pause.Initialize();
       

        _fsm.SetInit(main);
    }
}
