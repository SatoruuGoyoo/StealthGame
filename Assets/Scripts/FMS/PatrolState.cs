using Unity.VisualScripting;
using UnityEngine;

public class PatrolState : EnemyState
{
    private Transform[] waypoints;
    private int index = 0;

    public PatrolState(EnemyController controller) : base(controller)
    {
        waypoints = controller.waypoints;
    }

    public override void OnUpdate()
    {
        if (waypoints.Length == 0) return;

        Vector3 targetPos = waypoints[index].position;
        controller.MoveTo(targetPos);

        float distance = Vector3.Distance(controller.transform.position, targetPos);
        if (distance < 0.5f)
        {
            index = (index + 1) % waypoints.Length;
        }

        if (controller.CanSeeTarget())
        {
            controller.ChangeState(new PursuitState(controller));
        }
    }
}
