using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public GameObject entityUI;
    EnemyModel model;

    private void Awake()
    {
        model = GetComponent<EnemyModel>();
    }

    private void Update()
    {
        entityUI.SetActive(model.DetectingEntity);
    }

}
