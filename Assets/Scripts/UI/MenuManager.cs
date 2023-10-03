using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//로비의 메뉴를 열고 닫는 것을 관리하는 스크립트 
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance; //어디서든 접근 가능하게 싱글톤으로

    [SerializeField] Menu[] menus;

    public void Awake()
    {
        Instance = this;
    }

    //스트링 또는 메뉴 클래스를 파라미터로 하는 OpenMenu 함수
    public void OpenMenu(string menuName)
    {
        //모든 메뉴에 대해 일일이 참조를 할당하고 저장할 필요 없이, string 키 사용
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuNames == menuName) //열고자 하는 메뉴 오픈
            {
                menus[i].Open();
            }
            else if (menus[i].isOpen)
            {
                CloseMenu(menus[i]); //열고자 하는 메뉴 아닌, 이미 열려있는 메뉴 클로즈
            }
        }
    }

    public void OpenMenu(Menu menu)
    {//(메뉴 클래스는 레퍼런스를 용이하게 하기 위해서...?)

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen) //열고자 하는 메뉴가 이미 열려있는 상태라면 해당 메뉴 클로즈
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }


    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

}
