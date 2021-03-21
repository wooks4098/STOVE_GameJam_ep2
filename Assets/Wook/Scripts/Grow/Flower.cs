using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    float time;
    public float MyScale_x;
    public float MyScale_y;

    public Sprite[] sprite;
    SpriteRenderer spriterenderer;
    GameObject Camera;

    public void Awake()
    {
        time = 0;
        MyScale_x = 0;
        MyScale_y = 0;

        int rand;
        rand = Random.Range(0, 40);
        spriterenderer = GetComponentInChildren<SpriteRenderer>();
        spriterenderer.sprite = sprite[rand];
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

        if (Time.deltaTime - Trunk_.start_time < 1)       //현재시간-꽃이켜진시간 < 2초
            transform.localScale = new Vector3(MyScale_x, MyScale_y, transform.localScale.z);
    }
    private void LateUpdate()
    {
        Look_Camera();
    }
    void Look_Camera()
    {
        Vector3 vec = Camera.transform.position - transform.position;

        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q;
        var rot = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);
    }
}
