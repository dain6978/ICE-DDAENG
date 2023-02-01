using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; //���콺 Ŀ�� �Ⱥ��̰�
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ���� �߾ӿ� ����

    }

    // Update is called once per frame
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

    void OnCursor()
    {
        Cursor.lockState = CursorLockMode.None; //�ϴ�.. ���Ǹ� ���ؼ� none���� �ߴµ� ���߿��� confined�� �ٲٱ�..?
        Cursor.visible = true;
    }

    void OffCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
