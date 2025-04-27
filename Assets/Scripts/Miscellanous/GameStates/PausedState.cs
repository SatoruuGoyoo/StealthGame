using UnityEngine;

public class PausedState : State<GameState>
{
   
    public override void Enter()
    {
        Debug.Log("Juego en pausa");
       PauseUI.Instance.ShowPauseText();
        Time.timeScale = 0f; 
    
    }

    public override void Exit()
    {
        Debug.Log("Continuando juego");
        PauseUI.Instance.HidePauseText();
        Time.timeScale = 1f; 
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateMachine.Transition(GameState.InGame);
    
        }
    }
}
