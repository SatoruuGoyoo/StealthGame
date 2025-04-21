using UnityEngine;

public class EnemyController : MonoBehaviour
{
    LineOfSight _los;

    private void Awake()
    {
        _los = new LineOfSight();
    }


}
