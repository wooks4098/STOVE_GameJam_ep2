using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform cameraArm;//카메라 기준
    public Camera charactercamera;//카메라

    public bool isDragging = false;

    Animator animator;//캐릭터 애니메이션

    void Start()
    {
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            isDragging = true;
        if (Input.GetMouseButtonUp(0))
            isDragging = false;
        if (this.isDragging)
            LookAround();
        CameraZoom();
    }

    void CameraZoom()//카메라 줌
    {
        var scroll = Input.mouseScrollDelta;
        charactercamera.fieldOfView = Mathf.Clamp(charactercamera.fieldOfView - scroll.y * 2f, 30f, 65f);

    }

    
    void LookAround()//카메라 무빙
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * 3f, 0);//마우스 좌표 받기
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 45f);
        }
        else
        {
            // x = Mathf.Clamp(x, 335f, 361f);
            x = Mathf.Clamp(x, 345f, 371f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        // transform.RotateAround()
    }
}
