using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Col_Group : MonoBehaviour
{
    Point MyPos;
    Point Search_pos;
    int Trunk_Count = 1;
    int Trunk_Max_Count = 10;

    //Trunk 성장관련
    public float Growth_Scale_x;
    public float Growth_Scale_y;
    public float Growth_Position_y;
    public float Growth_Time;
    int Flower_Add_Min_Level;
    int Flower_Add_Rate;
    public GameObject PreFab;

    //확산관련
    public int Diffusion_Count;//확신 시도시 뿌리는 씨앗 개수
    public int Try_Diffusion_Count;//확산을 시도할 수 있는 횟수


    public float time;
    public float Diffusion_Time;

    public Cylinder cylinder;

    public JSONObject init;                     //특정 세포의 정보         init과 data중 편한 쪽으로 쓰시면될거같습니다.
    List<Dictionary<string, object>> data;
    GameObject newObject;


    void Start()
    {
        
    }

    private void Awake()
    {
        if (Trunk_Count == 1)
        {
            loadCsv();
            init = Initialize(9);       //100 알악세포 정보 들고오기
        }
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


            Search_pos.x = UnityEngine.Random.Range(-1, 2) + MyPos.x;
            Search_pos.y = UnityEngine.Random.Range(-1, 2) + MyPos.y;

            if (!((Search_pos.x >= 0 && Search_pos.x <= 15) && (Search_pos.y >= 0 && Search_pos.y <= 15)))
                continue;

            if(MyPos.x != Search_pos.x && MyPos.y != Search_pos.y)
                break;

            
        }

    }


    public void Creat_Trunk()
    {
        if (Trunk_Count >= Trunk_Max_Count)
            return;

        newObject = Instantiate(PreFab);
        newObject.GetComponent<Trunk_>().SetChildData(Trunk_Count
            , Growth_Scale_x, Growth_Scale_y, Growth_Time
            , Growth_Position_y, Flower_Add_Min_Level, Flower_Add_Rate);
        newObject.transform.parent = gameObject.transform;
        newObject.transform.localPosition = new Vector3(0, 0, 0);
        Trunk_Count++;


    }

    void loadCsv()
    {
        data = CSVReader.Read("cell_info");

        for (var i = 0; i < data.Count; i++)
        {
            List<string> keyList = new List<string>(data[i].Keys);
            string add = "";
            for (var j = 0; j < keyList.Count; j++)
            {
                if (j != 0)
                    add += ", ";
                add += data[i][keyList[j]].ToString();
            }
            //Debug.Log("index " + i.ToString() + " : " + add);
        }
    }

    JSONObject Initialize(int i)
    {
        JSONObject jo = new JSONObject(JSONObject.Type.OBJECT);
        float baseNumber = Convert.ToSingle(Math.Log(PreFab.transform.localScale.y) / PreFab.transform.localScale.y);

        System.Random rnd = new System.Random();
        jo.AddField("type", data[i]["Col_Type_Color"].ToString() + "_" + data[i]["Col_Type_Karma"].ToString());        //세포명
        Trunk_Max_Count = int.Parse(data[i]["Trunk_In_Col"].ToString());
        jo.AddField("def_cnt", int.Parse(data[i]["Trunk_In_Col"].ToString()));                                          //이 세포줄기에서 생길수 있는 줄기갯수
        Flower_Add_Min_Level = int.Parse(data[i]["Flower_In_Col_Trunkcondition"].ToString());        // 꽃이 생성될 수 있는 Trunk_Lv의 조건
        Flower_Add_Rate = (int)Math.Round(Convert.ToDouble(data[i]["Flower_In_Col_Rate"].ToString()) * 100);                                       //꽃이 생성될 확률
        Growth_Scale_x = Convert.ToSingle(rnd.NextDouble() * (Convert.ToDouble(data[i]["Trunk_Scale_X_Growth_Max"].ToString()) - 
            Convert.ToDouble(data[i]["Trunk_Scale_X_Growth_Min"].ToString())) + 
            Convert.ToDouble(data[i]["Trunk_Scale_X_Growth_Min"].ToString()));
        //x축으로 늘어날 최대 크기
        Growth_Scale_y = Convert.ToSingle(rnd.NextDouble() * (Convert.ToDouble(data[i]["Trunk_Scale_Y_Growth_Max"].ToString()) - 
            Convert.ToDouble(data[i]["Trunk_Scale_Y_Growth_Min"].ToString())) + 
            Convert.ToDouble(data[i]["Trunk_Scale_Y_Growth_Min"].ToString()));
        //y축으로 늘어날 최대 크기
        //Growth_Time = baseNumber;           //커지는속도
        Growth_Position_y = Convert.ToSingle(rnd.NextDouble() * (Convert.ToDouble(data[i]["Trunk_Build_Position_Max"].ToString()) - 
            Convert.ToDouble(data[i]["Trunk_Build_Position_Min"].ToString())) + 
            Convert.ToDouble(data[i]["Trunk_Build_Position_Min"].ToString()));
        //새로운 트렁크가 붙을 위치
        jo.AddField("smalltrunkTrunkLvMax", data[i]["Smalltrunk_Trunk_Lv_Max"].ToString());                             //작은 브랜치가 붙을수있는 최대 부모트렁크의 높이
        jo.AddField("smallTrunkCount", (rnd.NextDouble() * (Convert.ToDouble(data[i]["Smalltrunk_Max"].ToString()) - Convert.ToDouble(data[i]["Smalltrunk_Min"].ToString())) + Convert.ToDouble(data[i]["Smalltrunk_Min"].ToString())).ToString());
        //한 트렁크에 붙을 최대 브랜치의 갯수
        jo.AddField("smalltrunkRate", data[i]["Smalltrunk_Rate"].ToString());                                           //작은 브랜치가 붙을 확률

        return jo;
    }
}


