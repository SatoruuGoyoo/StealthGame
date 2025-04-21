using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel _model;
    private LineOfSight _los;

    private void Awake()
    {
        _los = new LineOfSight();
        _model = GetComponent<EnemyModel>();
    }

    private void Update()
    {
        var target = _model.CheckTarget();
        if (_los.LoS(_model.transform, _model.CheckTarget(), _model.range, _model.angle, _model.obstacleMask))
        {
            print("Target in range and angle, no obstacle");
        }
        else
        {
            print("Target out of range or angle, or obstacle in the way");
        }
        
    }

  

}
