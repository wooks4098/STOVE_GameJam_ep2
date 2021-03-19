using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Picker : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private ColorPicker picker;
    private Image image;

    private void Awake()
    {
        picker = GetComponentInParent<ColorPicker>();
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, eventData.position, eventData.pressEventCamera, out position);
        Debug.Log(position);
        picker.SetColor(new System.Drawing.Point((int)position.x, (int)position.y));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, eventData.position, eventData.pressEventCamera, out position);
        Debug.Log(position);
        picker.SetColor(new System.Drawing.Point((int)position.x, (int)position.y));
    }
}
