using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam; 

    void Update()
    {
        if (cam == null) //ī�޶� null�̶�� ���� ��򰡿��� camera�� ã�Ƽ� �Ҵ�
            cam = FindObjectOfType<Camera>();

        if (cam == null) //���� �� ī�޶� null�̶��, �� ������ ī�޶� ã�� �� �����ٸ�
            return;

        transform.LookAt(cam.transform); //�׻� ī�޶�(��ũ��)�� �ٶ󺸵���
        transform.Rotate(Vector3.up * 180);
    }
}
