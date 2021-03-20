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
        colorCountText.text = "Color : " + chalet.SelectedColorCount.ToString();
        pixelCountText.text = "Pixel : " + chalet.PixelCount.ToString();
        costText.text = "Cost required for apply : " + chalet.Cost.ToString();
        currentCostText.text = "Current Cost : " + UserData.Instance.GetUserData.Cost;
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
