using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorCost : MonoBehaviour
{
    [SerializeField]
    private Chalet chalet;
    [SerializeField]
    private Text colorCountText;
    [SerializeField]
    private Text pixelCountText;
    [SerializeField]
    private Text costText;
    [SerializeField]
    private Text currentCostText;

    void Update()
    {
        colorCountText.text = "배양액 종류 : " + chalet.SelectedColorCount.ToString();
        pixelCountText.text = "사용량 : " + chalet.PixelCount.ToString();
        costText.text = "필요한 연구점수 : " + chalet.Cost.ToString();
        currentCostText.text = "현재 연구점수 : " + UserData.Instance.GetUserData.Cost;
        if(UserData.Instance.GetUserData.Cost < chalet.Cost)
        {
            currentCostText.color = Color.red;
        }
        else
        {
            currentCostText.color = Color.white;
        }
    }
}
