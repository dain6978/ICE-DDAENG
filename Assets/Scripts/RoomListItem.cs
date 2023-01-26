using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

// ���� �̸��� ǥ���ϰ�, ��ư Ŭ�� -> �ش� �뿡 ����
public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomNameText;

    public RoomInfo info;

    // ������ ������ ���� �̸� ǥ��
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        roomNameText.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
