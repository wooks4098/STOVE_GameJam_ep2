using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Camera : MonoBehaviour
{
    GameObject Camera;

    private void Awake()
    {
        Camera = GameObject.FindWithTag("MainCamera");
    }

    private void LateUpdate()
    {
        Vector3 vec = Camera.transform.position - transform.position;
       
        //vec = new Vector3(0, vec.y, 0);
        Quaternion q = Quaternion.LookRotation(vec);

        transform.rotation = q;
        var rot = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);

        //transform.rotation = Quaternion.Euler(0, q.y,0);
    }
    private void Update()
    {
       

    }

}
