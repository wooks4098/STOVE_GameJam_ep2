using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk_ : MonoBehaviour
{
    //세대
    public int Number;

    bool IsGrowingUp;
    public bool IsFlower;
    //스케일값
    private float MyScale_x;
    private float MyScale_y;
    [Header("스케일값")]

    public float Scale_x_Growth;
    public float Scale_y_Growth;
    public float Growth_time;
    [Space(10f)]
    //포지션값
    private float MyPos_x;
    [SerializeReference]
    private float MyPos_y;
    [Header("포지션 값")]

    public float Pos_x_Growth;
    public float Pos_y_Growth;
    [Space(10f)]





    public float time;

    public GameObject Edge;
    public GameObject Flower;
    GameObject Camera;

    private void Awake()
    {
        Camera = GameObject.FindWithTag("MainCamera");
        IsGrowingUp = true;
        IsFlower = false;
        //transform.localPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        time += Time.deltaTime;


        //성장
        if (time < Growth_time)
        {
            Scale_Growth();
            Pos_Growth();

        }
        else
        {
            if(IsGrowingUp)
            {
                if(IsFlower)
                {
                    Flower.SetActive(true);
                }
                else
                {
                    Edge.SetActive(false);

                    transform.GetComponentInParent<Col_Group>().Creat_Trunk();//Creat_Trunk();
                    IsGrowingUp = false;
                }

            }

        }




    }

    private void LateUpdate()
    {
        Look_Camera();

    }


    void Pos_Growth()
    {
        //MyPos_x = Parents_Pos_x * Pos_x_Growth;
        //MyPos_y = Parents_Pos_y * Pos_y_Growth;
        
        transform.localPosition = new Vector3(0, MyPos_y, 0);
    }

    void Scale_Growth()
    {
        if(Number == 0)
        {
            MyScale_x = Mathf.Lerp(0, Scale_x_Growth, time / Growth_time);
            MyScale_y = Mathf.Lerp(0, Scale_y_Growth, time / Growth_time);
        }
        else
        {
            MyScale_x = Mathf.Lerp(Scale_x_Growth / 1.33f, Scale_x_Growth, time / Growth_time);
            MyScale_y = Mathf.Lerp(Scale_y_Growth / 1.33f, Scale_y_Growth, time / Growth_time);
        }

        transform.localScale = new Vector3(MyScale_x, MyScale_y, transform.localScale.z);

    }
    public void SetChildData(int _Number, float _Scale_x_Growth, float _Scale_y_Growth, float _Growth_time, float _Pos_y_Growth, int _Flower_Add_Min_Level, int _Flower_Add_Rate)
    {

        Number = _Number;

        //Scale_x_Growth = _Scale_x_Growth;
        //Scale_y_Growth = _Scale_y_Growth;
        //Growth_time = _Growth_time;

        ////Pos_x_Growth = _Pos_x_Growth;
        //Pos_y_Growth = _Pos_y_Growth;

        Scale_x_Growth = Mathf.Pow(_Scale_x_Growth, (Number - 1));
        Scale_y_Growth = Mathf.Pow(_Scale_y_Growth, (Number - 1));
        Growth_time = _Growth_time;

        System.Random rnd = new System.Random();
        int rndN = rnd.Next(100);
        Debug.Log(rndN + ":" + _Flower_Add_Rate);
        if (Number >= _Flower_Add_Min_Level - 1 && rndN < _Flower_Add_Rate)
            IsFlower = true;

        MyPos_y = 1 * (Number - 1) * Mathf.Pow(_Pos_y_Growth, (Number - 1));


        //newObject.GetComponent<Trunk_>().SetChildData(Trunk_Count
        //   , Mathf.Pow((Trunk_Count - 1), Growth_Scale_x), Mathf.Pow((Trunk_Count - 1), Growth_Scale_y), 5f
        //   , 1 * (Trunk_Count - 1) * Mathf.Pow((Trunk_Count - 1), Growth_Position_y));
        //newObject.transform.parent = gameObject.transform;
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
