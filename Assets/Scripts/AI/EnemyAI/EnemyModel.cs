using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    [Header("Line Of Sight")]
    [Range(1, 360)]
    public float angle;
    public float range;

    [Header("Enemy Layer")]
    public LayerMask obstacleMask;

    [Header("Enemy Target")]
    [SerializeField] private Transform _target;

    public Transform CheckTarget()
    {
        return _target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);

    }
}
