using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Temperature : MonoBehaviour
{
    public Text text_Tem;
    public Text Water;

    //온도 24 ~ 26.5
    //습도 40 55
    public float tem;
    public float wat;
    float time;
    float time2;
    private void Awake()
    {
        tem = 24.6f;
        wat = 44.4f;
        time = 0;
        time2 = 0;
    }

    private void Update()
    {
        text_Tem.text = tem.ToString("F1")+"℃";

        Water.text = wat.ToString("F1") + "%";
        time += Time.deltaTime;

        if(time>= 5)
        {
            tem += Random.Range(-0.2f, 0.2f);
            time = 0;
        }
        time2 += Time.deltaTime;

        if (time2 >= 3)
        {
            time2 = 0;
            wat += Random.Range(-0.2f, 0.2f);
        }
    }

}
