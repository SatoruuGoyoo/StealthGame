using UnityEngine;

public class InGameState : State<GameState>
{
    public override void Enter()
    {
        Debug.Log("Jugando");
        Time.timeScale = 1f;
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StateMachine.Transition(GameState.Paused);
    }
}
