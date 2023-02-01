using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; //마우스 커서 안보이게
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 게임 중앙에 고정

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.visible) //커서가 보이는 상태면
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
        Cursor.lockState = CursorLockMode.None; //일단.. 편의를 위해서 none으로 했는데 나중에는 confined로 바꾸기..?
        Cursor.visible = true;
    }

    void OffCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
