using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�޴��� ���� ��ũ��Ʈ
//Menu�� UIView�̰�, MenuManager�� UIManager�� ����
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
