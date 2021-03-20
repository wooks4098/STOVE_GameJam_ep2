using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorCost : MonoBehaviour
{
    [SerializeField]
    private Chalet chalet;
    [SerializeField]
    private Text costText;

    
    void Update()
    {
        costText.text = "Cost : " + chalet.SelectedColorCount.ToString();
    }
}
