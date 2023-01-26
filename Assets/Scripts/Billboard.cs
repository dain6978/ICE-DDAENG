using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam; 

    void Update()
    {
        if (cam == null) //카메라가 null이라면 씬의 어딘가에서 camera를 찾아서 할당
            cam = FindObjectOfType<Camera>();

        if (cam == null) //만약 또 카메라가 null이라면, 즉 씬에서 카메라를 찾을 수 없었다면
            return;

        transform.LookAt(cam.transform); //항상 카메라(스크린)를 바라보도록
        transform.Rotate(Vector3.up * 180);
    }
}
