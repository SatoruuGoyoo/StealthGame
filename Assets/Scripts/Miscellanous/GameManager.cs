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
        var over = new GameOverState();
        var win = new VictoryState();


        main.AddTransition(GameState.Loading, loading);
        loading.AddTransition(GameState.InGame, game);
        game.AddTransition(GameState.Paused, pause);
        game.AddTransition(GameState.GameOver, over);
        game.AddTransition(GameState.Victory, win);
        pause.AddTransition(GameState.InGame, game);
        //over.AddTransition(GameState.MainMenu, main);
        over.AddTransition(GameState.Loading, loading);
        over.AddTransition(GameState.InGame, game);
        win.AddTransition(GameState.MainMenu, main);
        ;

        main.Initialize();
        loading.Initialize();
        game.Initialize();
        pause.Initialize();
        over.Initialize();
        win.Initialize();


        _fsm.SetInit(main);
    }
}
