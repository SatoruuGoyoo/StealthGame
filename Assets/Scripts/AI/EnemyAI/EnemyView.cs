using UnityEngine;

public class EnemyView : MonoBehaviour, ILook
{
    public float speedRot = 10;
    Rigidbody _rb;

    public void LookDir(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRot);
    }

    //public GameObject entityUI;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    //private void OnDestroy()
    //{
    //    var model = GetComponent<EnemyModel>();
    //    if (model.onChangeEntityUI != null)
    //    {
    //        model.onChangeEntityUI -= onChangeUI;
    //    }
    //}

    //void onChangeUI(bool v)
    //{
    //    entityUI.SetActive(v);
    //}




}
