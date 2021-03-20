using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class addCellEvent : MonoBehaviour
{
    public GameObject target;           //첫세포
    int count = 0;
    int def_cnt = 1;                    //몇개까지 늘어나는가
    public JSONObject init;                     //특정 세포의 정보         init과 data중 편한 쪽으로 쓰시면될거같습니다.
    List<Dictionary<string, object>> data;      //전체 세포들 정보

    // Start is called before the first frame update
    void Start()
    {
        loadCsv();                      //csv파일 파싱및 값 확인
    }

    // Update is called once per frame
    void Update()
    {
        if (count < def_cnt)
        {
            init = Initialize(0);       //100 알악세포 정보 들고오기
            count++;
        }
    }

    void loadCsv ()
    {
        data = CSVReader.Read("cell_info");
       
        for ( var i = 0; i < data.Count; i++ )
        {
            List<string> keyList = new List<string>(data[i].Keys);
            string add = "";
            for ( var j = 0; j < keyList.Count; j++ )
            {
                if (j != 0)
                    add += ", ";
                add += data[i][keyList[j]].ToString();
            }
            Debug.Log("index " + i.ToString() + " : " + add);
        }
    }

    JSONObject Initialize(int i)
    {
        JSONObject jo = new JSONObject(JSONObject.Type.OBJECT);
        RectTransform rt = (RectTransform)target.transform;
        double baseNumber = Math.Log(rt.rect.height * target.transform.localScale.y) / rt.rect.height * target.transform.localScale.y;

        System.Random rnd = new System.Random();
        jo.AddField("type", data[i]["Col_Type_Color"].ToString() + "_" + data[i]["Col_Type_Karma"].ToString() );        //세포명
        def_cnt = int.Parse(data[i]["Trunk_In_Col"].ToString());
        jo.AddField("def_cnt", int.Parse(data[i]["Trunk_In_Col"].ToString()));                                          //이 세포줄기에서 생길수 있는 줄기갯수
        jo.AddField("flowerInColTrunkcondition", int.Parse(data[i]["Flower_In_Col_Trunkcondition"].ToString()));        // 꽃이 생성될 수 있는 Trunk_Lv의 조건
        jo.AddField("flowerInColRate", data[i]["Flower_In_Col_Rate"].ToString());                                       //꽃이 생성될 확률
        jo.AddField("chgX", (rnd.NextDouble() * (Convert.ToDouble(data[i]["Trunk_Scale_X_Growth_Max"].ToString()) - Convert.ToDouble(data[i]["Trunk_Scale_X_Growth_Min"].ToString())) + Convert.ToDouble(data[i]["Trunk_Scale_X_Growth_Min"].ToString())).ToString());
                //x축으로 늘어날 최대 크기
        jo.AddField("chgY", (rnd.NextDouble() * (Convert.ToDouble(data[i]["Trunk_Scale_Y_Growth_Max"].ToString()) - Convert.ToDouble(data[i]["Trunk_Scale_Y_Growth_Min"].ToString())) + Convert.ToDouble(data[i]["Trunk_Scale_Y_Growth_Min"].ToString())).ToString());
                //y축으로 늘어날 최대 크기
        jo.AddField("posY", (rnd.NextDouble() * (Convert.ToDouble(data[i]["Trunk_Build_Position_Max"].ToString()) - Convert.ToDouble(data[i]["Trunk_Build_Position_Min"].ToString())) + Convert.ToDouble(data[i]["Trunk_Build_Position_Min"].ToString())).ToString());
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
