using UnityEngine;

// NPCModel is a class that represents a non-player character (NPC) in the game.
// It inherits from PlayerModel and implements the IMove and IAttack interfaces.

public class NPCModel : PlayerModel
{
    [Header("Attack Settings")]
    public float attackRange;

    [Header("LayerMask")]
    public LayerMask enemyMask;

    ObstacleAvoidance _obs;
    ILook _look;

    public bool lostPlayerTooLong = false;


    protected override void Awake()
    {
        _obs = GetComponent<ObstacleAvoidance>();
        _look = GetComponent<ILook>();
        base.Awake();
    }
    public override void Attack()
    {
        var colls = Physics.OverlapSphere(Position, attackRange, enemyMask);
        for (int i = 0; i < colls.Length; i++)
        {
            
            GameObject.Destroy(colls[i].gameObject);
            DefeatHandler.Instance.TriggerDefeat();

        }
        base.Attack();
    }
    public override void Move(Vector3 dir)
    {
        
        base.Move(dir);
    }
}
