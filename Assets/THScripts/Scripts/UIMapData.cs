using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapData : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private MapData mapData;

    public void Init(MapData mapData)
    {
        this.mapData = mapData;
        image.sprite = mapData.sprite;
    }

    public void Click()
    {
        Debug.Log("Clicked! : " + mapData.SpriteStr);
    }
}
