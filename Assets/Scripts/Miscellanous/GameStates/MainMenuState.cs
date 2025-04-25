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
        Debug.Log("🟢 Estado: MainMenu");
        if (mainMenu != null)
            mainMenu.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        Debug.Log("⏭ Saliendo de MainMenu");
        if (mainMenu != null)
            mainMenu.gameObject.SetActive(false);
    }
}
