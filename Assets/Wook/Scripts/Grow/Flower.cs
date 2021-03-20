using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    float time;
    public float MyScale_x;
    public float MyScale_y;
    public void Awake()
    {
        time = 0;
        MyScale_x = 0;
        MyScale_y = 0;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        MyScale_x = Mathf.Lerp(0, 1, time / 5);
        MyScale_y = Mathf.Lerp(0, 1, time / 5);

        if (time < 1.7)
            transform.localScale = new Vector3(MyScale_x, MyScale_y, transform.localScale.z);
    }
}
