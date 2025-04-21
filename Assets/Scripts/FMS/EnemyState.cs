using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController controller;

    public EnemyState(EnemyController controller)
    {
        this.controller = controller;
    }

    public virtual void OnEnter()
    {
        // Called when entering the state
    }
    public virtual void OnExit()
    {
        // Called when exiting the state
    }
    public virtual void OnUpdate()
    {
        // Called every frame while in the state
    }   
}
