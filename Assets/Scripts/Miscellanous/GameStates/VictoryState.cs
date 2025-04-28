using UnityEngine;

public class VictoryState : State<GameState>
{

    public override void Enter()
    {

      
        Time.timeScale = 0f;

    }

    public override void Exit()
    {
       
    }

    public override void Execute()
    {
    }

    
}
