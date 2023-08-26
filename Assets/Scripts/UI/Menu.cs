using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//메뉴에 대한 스크립트
//Menu가 UIView이고, MenuManager가 UIManager인 형식
public class Menu : MonoBehaviour
{
    public string menuNames;
    public bool isOpen;

    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
