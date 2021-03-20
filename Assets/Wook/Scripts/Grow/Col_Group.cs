using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Col_Group : MonoBehaviour
{
    int Trunk_Count = 1;
    int Trunk_Max_Count = 1;

    public float Growth_Scale_x;
    public float Growth_Scale_y;

    public float Growth_Position_y;
    public float Growth_Time;
    int Flower_Add_Min_Level;
    int Flower_Add_Rate;
    public GameObject PreFab;

    public JSONObject init;                     //특정 세포의 정보         init과 data중 편한 쪽으로 쓰시면될거같습니다.
    List<Dictionary<string, object>> data;
    GameObject newObject;

    private void Awake()
    {
        Creat_Trunk();
    }

    private void Update()
    {
        
    }


    public void Creat_Trunk()
    {
        if (Trunk_Count >= Trunk_Max_Count)
            return;

        newObject = Instantiate(PreFab);
        if (Trunk_Count == 1)
        {
            loadCsv();
            init = Initialize(0);       //100 알악세포 정보 들고오기
        }
        newObject.GetComponent<Trunk_>().SetChildData(Trunk_Count
            , Growth_Scale_x, Growth_Scale_y, Growth_Time
            , Growth_Position_y, Flower_Add_Min_Level, Flower_Add_Rate);
        newObject.transform.parent = gameObject.transform;
        //newObject.GetComponent<Trunk_>().SetChildData(Trunk_Count
        //   , Mathf.Pow((Trunk_Count - 1), Growth_Scale_x), Mathf.Pow((Trunk_Count - 1), Growth_Scale_y), 5f
        //   , 1 * (Trunk_Count - 1) * Mathf.Pow((Trunk_Count - 1), Growth_Position_y));
        //newObject.transform.parent = gameObject.transform;
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
        RectTransform rt = (RectTransform)PreFab.transform;
        float baseNumber = Convert.ToSingle(Math.Log(rt.rect.height * PreFab.transform.localScale.y) / rt.rect.height * PreFab.transform.localScale.y);

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
        Growth_Time = baseNumber;           //커지는속도
        Growth_Position_y = Convert.ToSingle(rnd.NextDouble() * (Convert.ToDouble(data[i]["Trunk_Build_Position_Max"].ToString()) - 
            Convert.ToDouble(data[i]["Trunk_Build_Position_Min"].ToString())) + 
            Convert.ToDouble(data[i]["Trunk_Build_Position_Min"].ToString()));
        //새로운 트렁크가 붙을 위치
        jo.AddField("smalltrunkTrunkLvMax", data[i]["Smalltrunk_Trunk_Lv_Max"].ToString());                             //작은 브랜치가 붙을수있는 최대 부모트렁크의 높이
        jo.AddField("smallTrunkCount", (rnd.NextDouble() * (Convert.ToDouble(data[i]["Smalltrunk_Max"].ToString()) - Convert.ToDouble(data[i]["Smalltrunk_Min"].ToString())) + Convert.ToDouble(data[i]["Smalltrunk_Min"].ToString())).ToString());
        //한 트렁크에 붙을 최대 브랜치의 갯수
        jo.AddField("smalltrunkRate", data[i]["Smalltrunk_Rate"].ToString());                                           //작은 브랜치가 붙을 확률

        return jo;
        /*newObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + baseNumber * chgY + count * chgY, target.transform.position.z);
        newObject.transform.localScale = new Vector3(1, 1, 1);
        newObject.transform.SetParent(target.transform);*/
    }
}


