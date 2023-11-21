using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager.isEnd)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (cam == null) //ī�޶� null�̶�� ���� ��򰡿��� camera�� ã�Ƽ� �Ҵ�
            cam = FindObjectOfType<Camera>();

        if (cam == null) //���� �� ī�޶� null�̶��, �� ������ ī�޶� ã�� �� �����ٸ�
            return;

        transform.LookAt(cam.transform); //�׻� ī�޶�(��ũ��)�� �ٶ󺸵���
        transform.Rotate(Vector3.up * 180);
    }
}
