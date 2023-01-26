using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

// 룸의 이름을 표시하고, 버튼 클릭 -> 해당 룸에 가입
public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomNameText;

    public RoomInfo info;

    // 서버에 생성한 룸의 이름 표시
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
