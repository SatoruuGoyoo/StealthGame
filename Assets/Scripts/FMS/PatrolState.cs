using Unity.VisualScripting;
using UnityEngine;

public class PatrolState : EnemyState
{
    private Transform[] waypoints;
    private int index = 0;
    private int direction = 1;

    public PatrolState(EnemyController controller) : base(controller)
    {
        waypoints = controller.Waypoints;
    }

    public override void Enter()
    {
        Debug.Log("Entering Patrol State");
    }

    public override void Update()
    {
        if(model.DetectingEntity)
        {
            controller.ChangeState(new PursuitState(controller));
            return;
        }
    }

    Vector3 targetPos = waypoints[index].position;
    controller.MoveTo(targetPos);

    float distance = Vector3.Distance(controller.transform.position, targetPos);
    If(distance< 0.2f)
    {
        index += direction;
        if (index >= waypoints.Length || index < 0)
        {
            direction *= -1;
            index += direction;
        }
    }
}
