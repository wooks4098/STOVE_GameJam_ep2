using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragger : MonoBehaviour, IDragHandler, IDropHandler
{
    public CameraMove CamController;

    public void OnDrag(PointerEventData data)
    {
        CamController.isDragging = true;
    }

    public void OnDrop(PointerEventData data)
    {
        CamController.isDragging = false;
    }

}
