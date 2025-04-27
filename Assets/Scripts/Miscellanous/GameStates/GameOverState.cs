using UnityEngine;

public class GameOverState : State<GameState>
{
    public override void Enter()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }

    public override void Exit()
    {
        Debug.Log("Intentando de nuevo");

    }

    public override void Execute()
    {

      
    }

}
