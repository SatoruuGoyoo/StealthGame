using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol<T>: NPCSBase<T>
{
    [Tooltip("Lista de waypoints que el NPC recorrerá en orden")]
    public List<Transform> Waypoints;

    [Tooltip("Velocidad de patrulla")]
    public float PatrolSpeed = 2f;

    [Tooltip("Distancia mínima para considerar que llegó al waypoint")]
    public float ArrivalThreshold = 0.1f;

    private int _currentIndex = 0;

    private Transform _model;

    public override void Enter()
    {
        base.Enter();

        if (Waypoints == null || Waypoints.Count == 0)
        {
            Debug.LogWarning("NPCSPatrol: No hay waypoints asignados.");
            return;
        }

        _currentIndex = 0;
        MoveToCurrentWaypoint();
    }

    public override void Execute()
    {
        base.Execute();

        if (Waypoints == null || Waypoints.Count == 0)
            return;

        // Calcula distancia al waypoint actual
        var targetPos = Waypoints[_currentIndex].position;
        float dist = Vector3.Distance(_model.position, targetPos);

        // Si llegó al waypoint, avanza al siguiente
        if (dist <= ArrivalThreshold)
        {
            _currentIndex = (_currentIndex + 1) % Waypoints.Count;
            MoveToCurrentWaypoint();
        }
    }

    private void MoveToCurrentWaypoint()
    {
        var nextPos = Waypoints[_currentIndex].position;
        Vector3 direction = (nextPos - _model.position).normalized;
        _move.Move(direction * PatrolSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
