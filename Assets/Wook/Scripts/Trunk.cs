using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    private float Scale_x;
    public float Scale_x_max;
    private float Scale_y;
    public float Scale_y_max;
    public float Pos_x;
    public float Pos_y;
    public float Growth_time;

    public float time;
    GameObject Camera;

    private void Awake()
    {
        time = 0;
        Camera = GameObject.FindWithTag("MainCamera");

    }
    void Start()
    {
        //StartCoroutine(Grow_y());
        //StartCoroutine(Grow_x());
       

    }


    void Update()
    {
        Look_Camera();
        time += Time.deltaTime;
        if(time <Growth_time)
        {
            Scale_y = Mathf.Lerp(0, Scale_y_max, time/ Growth_time);
            Scale_x = Mathf.Lerp(0, Scale_x_max, time/ Growth_time) ;
            transform.localScale = new Vector3(Scale_y, Scale_x, transform.localScale.z);
        }
    }

    void Look_Camera()
    {
        Vector3 vec = Camera.transform.position - transform.position;

        
        Quaternion q = Quaternion.LookRotation(vec);

        transform.rotation = q;
    }

    //IEnumerator Grow_x()
    //{
    //    float time = 0;
    //    while(Scale_x < Scale_x_max)
    //    {
    //        time += Time.deltaTime / Growth_Speed;
    //        Scale_x = Mathf.Lerp(0, Scale_x_max, time);
    //        transform.localScale = new Vector3(Scale_x, transform.localScale.y, transform.localScale.z);
    //    }

    //    yield return null;
    //}

    //IEnumerator Grow_y()
    //{
    //    float time = 0;
    //    float TimeCheck = Growth_time;
    //    float Scale_y = 0;
    //    while (Scale_y < Scale_y_max)
    //    {
    //        time += Time.deltaTime / TimeCheck;
    //        Scale_y = Mathf.Lerp(0, Scale_x_max, time);
    //        transform.localScale = new Vector3(transform.localScale.x, Scale_y, transform.localScale.z);
    //    }

    //    yield return null;
    //}

}
