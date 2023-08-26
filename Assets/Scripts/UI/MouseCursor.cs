using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false; //���콺 Ŀ�� �Ⱥ��̰�
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ���� �߾ӿ� ����

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.visible) //Ŀ���� ���̴� ���¸�
            {
                OffCursor();
            }
            else
            {
                OnCursor();
            }
        }
    }

    public void OnCursor()
    {
        Cursor.lockState = CursorLockMode.None; //�ϴ�.. ���Ǹ� ���ؼ� none���� �ߴµ� ���߿��� confined�� �ٲٱ�..?
        Cursor.visible = true;
    }

    public void OffCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
