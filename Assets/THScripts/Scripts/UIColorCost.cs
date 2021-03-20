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

    void Update()
    {
        costText.text = "Cost : " + chalet.Cost.ToString();
        colorCountText.text = "Color : " + chalet.SelectedColorCount.ToString();
        pixelCountText.text = "Pixel : " + chalet.PixelCount.ToString();
    }
}
