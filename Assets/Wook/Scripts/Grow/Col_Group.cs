using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col_Group : MonoBehaviour
{
    Point MyPos;
    Point Search_pos;
    public int Trunk_Count = 1;

    //Trunk 성장관련
    public float Growth_Scale_x;
    public float Growth_Scale_y;
    public float Growth_Position_y;
    public float Growth_Time;
    public GameObject PreFab;

    //확산관련
    public int Diffusion_Count;//확신 시도시 뿌리는 씨앗 개수
    public int Try_Diffusion_Count;//확산을 시도할 수 있는 횟수


    public float time;
    public float Diffusion_Time;

    public Cylinder cylinder;

    private void Awake()
    {
        Creat_Trunk();
        Diffusion_Time = 3f;
        transform.localPosition = new Vector3(0, 0, 0);

    }

    private void Update()
    {
        Diffusion();
    }

    public void SetPos(int x, int y)
    {
        MyPos.x = x;
        MyPos.y = y;

        //transform.parent.localPosition = new Vector3(0, 0, 0);
    }

    void Diffusion()
    {
        if (Trunk_Count >= 3)// Trunk가 3이상인가
        {
            if (Try_Diffusion_Count > 0) // 확산을 시도할 수 있는가?
            {
                time += Time.deltaTime;
                if (time >= Diffusion_Time) //확산 가능한 시간인가?
                {
                    for (int i = 0; i < Diffusion_Count; i++)
                    {
                        Rand_Pos();
                        if(!cylinder.Search(Search_pos))
                        {
                            cylinder.Creat_Col(Search_pos);
                            time = 0;
                        }
                    }
                }
            }
        }
    }
    void Rand_Pos()
    {
        int safety = 10;

        while(true)
        {
            safety--;
            if (safety <= 0)
            {
                Search_pos.x = 0;
                Search_pos.y = 0;
                break;
            }


            Search_pos.x = Random.Range(-1, 2) + MyPos.x;
            Search_pos.y = Random.Range(-1, 2) + MyPos.y;

            if (!((Search_pos.x >= 0 && Search_pos.x <= 15) && (Search_pos.y >= 0 && Search_pos.y <= 15)))
                continue;

            if(MyPos.x != Search_pos.x && MyPos.y != Search_pos.y)
                break;

            
        }

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
        Trunk_Count++;


    }
}


