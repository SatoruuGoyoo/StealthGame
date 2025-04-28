using UnityEngine;

public class MainMenuState : State<GameState>
{
    MainMenu mainMenu;

    public override void Initialize(params object[] p)
    {
        mainMenu = GameObject.FindFirstObjectByType<MainMenu>();
    }

    public override void Enter()
    {
       
        if (mainMenu != null)
            mainMenu.gameObject.SetActive(true);
    }

    public override void Exit()
    {
       
        if (mainMenu != null)
            mainMenu.gameObject.SetActive(false);
    }
}
