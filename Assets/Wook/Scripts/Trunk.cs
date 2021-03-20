using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SPRITE { Trunk, Edge, Flower0, Flower_1};

public class Trunk : MonoBehaviour
{
    //세대
    public int Number;

    public bool isTrunk;

    //스케일값
    private float MyScale_x;
    private float MyScale_y;
    [Header("스케일값")]
    public float Parents_Scale_x;
    public float Parents_Scale_y;
    public float Scale_x_Growth;
    public float Scale_y_Growth;
    public float Growth_time;
    [Space(10f)]
    //포지션값
    private float MyPos_x;
    [SerializeReference]
    private float MyPos_y;
    [Header("포지션 값")]
    public float Parents_Pos_x;
    public float Parents_Pos_y;
    public float Pos_x_Growth;
    public float Pos_y_Growth;
    [Space(10f)]


    public Sprite[] sprites;


    //위로 자식
    public GameObject PreFab;
    public GameObject UpTrunk;
    //작은 자식
    GameObject[] Small_Trunk;


    [SerializeReference]
    private float time;
    GameObject Camera;

    private void Awake()
    {
        time = 0;
        if (Number == 0)
            isTrunk = true;
        else
            isTrunk = false;

        //스프라이트 조정
        if (isTrunk)
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[(int)SPRITE.Trunk];
        else
        {
            if(Number != 7)
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[(int)SPRITE.Edge];
            else
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[(int)SPRITE.Flower0];

        }


        Camera = GameObject.FindWithTag("MainCamera");

        if (isTrunk)//루트 Trunk일 경우에만 사용하는 자식 생성
        {
            GameObject newObject = Instantiate(PreFab);
            newObject.GetComponent<Trunk>().SetChildData(
               Number + 1, Parents_Scale_x * Scale_x_Growth, Parents_Scale_y * Scale_y_Growth, 1f, 1f, 5
                , transform.position.x, transform.position.y + transform.localScale.y, 1f, 1.1f);
            //newObject.transform.parent = gameObject.transform;
        }
    }
    private void Start()
    {
        Pos_Growth();
    }

    void Update()
    {
        time += Time.deltaTime;


        //성장
        if (time < Growth_time)
        {
            Scale_Growth();
            Pos_Growth();
            if (isTrunk)
                transform.GetChild(1).GetComponent<Trunk>().GetParentsPos_y(transform.position.y + transform.localScale.y);//부모의 이미지 높이와 y포지션을 더한값
        }
        if (Number != 0 && time > Growth_time && !isTrunk)
            Change_Trunk();


    }

    private void LateUpdate()
    {
        Look_Camera();

    }

    #region 성장관련

    //자식에게 부모 데이터 주는 함수
    public void SetChildData(int _Number, float _Parents_Scale_x, float _Parents_Scale_y, float _Scale_x_Growth, float _Scale_y_Growth, float _Growth_time,
        float _Parents_Pos_x, float _Parents_Pos_y, float _Pos_x_Growth, float _Pos_y_Growth)
    {

        Number = _Number;
        Parents_Scale_x = _Parents_Scale_x;
        Parents_Scale_y = _Parents_Scale_y;
        Scale_x_Growth = _Scale_x_Growth;
        Scale_y_Growth = _Scale_y_Growth;
        Growth_time = _Growth_time;
        Parents_Pos_x = _Parents_Pos_x;
        Parents_Pos_y = _Parents_Pos_y;
        Pos_x_Growth = _Pos_x_Growth;
        Pos_y_Growth = _Pos_y_Growth;
        isTrunk = false;
    }

    //자식에게 부모의 이미지 높이와 y포지션을 더한값을 주는 함수
    void GetParentsPos_y(float y)
    {
        Parents_Pos_y = y;
    }

    void Pos_Growth()
    {
        MyScale_x = 0;
        MyScale_y = 0;
        MyPos_x = Parents_Pos_x * Pos_x_Growth;
        MyPos_y = Parents_Pos_y * Pos_y_Growth;

        transform.position = new Vector3(MyPos_x, MyPos_y, 0);
    }

    void Scale_Growth()
    {

        MyScale_x = Mathf.Lerp(0, Parents_Scale_x * Scale_x_Growth, time / Growth_time);
        MyScale_y = Mathf.Lerp(0, Parents_Scale_y * Scale_y_Growth, time / Growth_time);
        transform.localScale = new Vector3(MyScale_x, MyScale_y, transform.localScale.z);
 
    }
    #endregion

    //다 자랐을 때 Trunk로 바꿔줌
    void Change_Trunk()
    {
        if(Number!= 0 && Number <= 8)
        {
            isTrunk = true;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[(int)SPRITE.Trunk];
            GameObject newObject = Instantiate(PreFab);
            newObject.GetComponent<Trunk>().SetChildData(
               Number + 1, Parents_Scale_x * Scale_x_Growth, Parents_Scale_y * Scale_y_Growth, 0.8f, 0.8f, 5
                , transform.position.x, transform.position.y + transform.localScale.y, 1f, 1.1f);
           // newObject.transform.parent = gameObject.transform;


            time = 0;
            MyScale_x = 0;
            MyScale_y = 0;
            transform.localScale = new Vector3(MyScale_x, MyScale_y, transform.localScale.z);
        }
    }

    


    //스프라이트가 카메라 바라보기
    void Look_Camera()
    {
        Vector3 vec = Camera.transform.position - transform.position;

        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q;
        var rot = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);
    }
    
    



}
