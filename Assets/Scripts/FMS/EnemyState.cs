using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController controller;
    protected EnemyModel model;

    public EnemyState(EnemyController controller)
    {
        this.controller = controller;
        this.model = controller.Model;
    }

    public virtual void Enter()
    {
        // Called when entering the state
    }
    public virtual void Exit()
    {
        // Called when exiting the state
    }
    public virtual void Update()
    {
        // Called every frame while in the state
    }   
}
