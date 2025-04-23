using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public GameObject entityUI;
    

    private void Awake()
    {
        //GetComponent<EnemyModel>().onChangeEntityUI += onChangeUI;
    }

    private void OnDestroy()
    {
        //var model = GetComponent<EnemyModel>();
        //if (model.onChangeEntityUI != null)
        //{
        //    model.onChangeEntityUI -= onChangeUI;
        //}
    }

    void onChangeUI(bool v)
    {
        entityUI.SetActive(v);
    }




}
