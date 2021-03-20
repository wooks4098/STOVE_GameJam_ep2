using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    //[SerializeField] private Transform characterBody;//캐릭터 몸통
    //[SerializeField] private Transform cameraArm;//카메라 기준
    //[SerializeField] private Camera charactercamera;//카메라

    //public bool isDragging = false;
    //private object fieldOfView;

    //private void Update()
    //{
    //    if (this.isDragging)
    //        LookAround();
    //    CameraZoom();
    //}


    //void CameraZoom()//카메라 줌
    //{
    //    var scroll = Input.mouseScrollDelta;
    //    charactercamera.fieldOfView = Mathf.Clamp(charactercamera.fieldOfView - scroll.y * 2f, 30f, 65f);

    //}



    //void LookAround()//카메라 무빙
    //{
    //    Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * 3f, Input.GetAxis("Mouse Y") * 3f);//마우스 좌표 받기
    //    Vector3 camAngle = cameraArm.rotation.eulerAngles;

    //    float x = camAngle.x - mouseDelta.y;
    //    if (x < 180f)
    //    {
    //        x = Mathf.Clamp(x, -1f, 45f);
    //    }
    //    else
    //    {
    //        // x = Mathf.Clamp(x, 335f, 361f);
    //        x = Mathf.Clamp(x, 345f, 371f);
    //    }

    //    cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    //    // transform.RotateAround()
    //}
}
