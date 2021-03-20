using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col_Group : MonoBehaviour
{
    public int Trunk_Count = 1;

    public float Growth_Scale_x;
    public float Growth_Scale_y;

    public float Growth_Position_y;
    public float Growth_Time;
    public GameObject PreFab;

    private void Awake()
    {
        Creat_Trunk();
    }

    private void Update()
    {
        
    }


    public void Creat_Trunk()
    {
        if (Trunk_Count >= 8)
            return;
        GameObject newObject = Instantiate(PreFab);
        newObject.GetComponent<Trunk_>().SetChildData(Trunk_Count
            , Growth_Scale_x, Growth_Scale_y, Growth_Time
            , Growth_Position_y);
        newObject.transform.parent = gameObject.transform;
        //newObject.GetComponent<Trunk_>().SetChildData(Trunk_Count
        //   , Mathf.Pow((Trunk_Count - 1), Growth_Scale_x), Mathf.Pow((Trunk_Count - 1), Growth_Scale_y), 5f
        //   , 1 * (Trunk_Count - 1) * Mathf.Pow((Trunk_Count - 1), Growth_Position_y));
        //newObject.transform.parent = gameObject.transform;
        Trunk_Count++;


    }
}


