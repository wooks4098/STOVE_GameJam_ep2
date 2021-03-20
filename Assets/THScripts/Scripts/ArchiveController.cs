using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveController : MonoBehaviour
{
    [SerializeField]
    private UIMapData mapDataPrefab;



    private void Start()
    {
        UserData.Instance.GetUserData.OnMapDataChanged += Init;
        Init(UserData.Instance.GetUserData.mapDatas);
    }

    private void OnDestroy()
    {
        if(UserData.Instance)
            UserData.Instance.GetUserData.OnMapDataChanged -= Init;
    }

    public void Init(List<MapData> datas)
    {
        UIMapData[] prevDatas = GetComponentsInChildren<UIMapData>();

        foreach(UIMapData data in prevDatas)
        {
            Destroy(data.gameObject);
        }

        for (int i = 0; i < datas.Count; i++)
        {
            UIMapData newData = Instantiate(mapDataPrefab, transform);
            newData.Init(datas[i]);
        }
    }
}
