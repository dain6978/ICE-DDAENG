using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�κ��� �޴��� ���� �ݴ� ���� �����ϴ� ��ũ��Ʈ 
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance; //��𼭵� ���� �����ϰ� �̱�������

    [SerializeField] Menu[] menus;

    public void Awake()
    {
        Instance = this;
    }

    //��Ʈ�� �Ǵ� �޴� Ŭ������ �Ķ���ͷ� �ϴ� OpenMenu �Լ�
    public void OpenMenu(string menuName)
    {
        //��� �޴��� ���� ������ ������ �Ҵ��ϰ� ������ �ʿ� ����, string Ű ���
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuNames == menuName) //������ �ϴ� �޴� ����
            {
                menus[i].Open();
            }
            else if (menus[i].isOpen)
            {
                CloseMenu(menus[i]); //������ �ϴ� �޴� �ƴ�, �̹� �����ִ� �޴� Ŭ����
            }
        }
    }

    public void OpenMenu(Menu menu)
    {//(�޴� Ŭ������ ���۷����� �����ϰ� �ϱ� ���ؼ�...?)

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen) //������ �ϴ� �޴��� �̹� �����ִ� ���¶�� �ش� �޴� Ŭ����
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
