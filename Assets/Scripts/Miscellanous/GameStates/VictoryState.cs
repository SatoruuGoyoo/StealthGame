using UnityEngine;

public class VictoryState : State<GameState>
{

    public override void Enter()
    {

        Debug.Log("Victoria!");
        Time.timeScale = 0f;

    }

    public override void Exit()
    {
        Debug.Log("Continuando...");
    }

    public override void Execute()
    {
    }

    
}
