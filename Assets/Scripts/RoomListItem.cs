using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

// 룸의 이름을 표시하고, 버튼 클릭 -> 해당 룸에 가입
public class RoomListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text roomNameText;

    [SerializeField] TMP_Text gameState_text;
    [SerializeField] TMP_Text playerCount_text;

    public RoomInfo info;

    // 서버에 생성한 룸의 이름 표시
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        roomNameText.text = _info.Name;
        UpdatePlayerCount(_info);
        ChangeGameState(_info);
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }

    public void UpdatePlayerCount(RoomInfo _info)
    {
        playerCount_text.text = _info.PlayerCount.ToString() + "/" + _info.MaxPlayers.ToString();

        if (_info.PlayerCount == _info.MaxPlayers)
            playerCount_text.color = Color.red;
        else
            playerCount_text.color = Color.black;
    }

    public void ChangeGameState(RoomInfo _info)
    {
        if (_info.IsOpen)
        {
            gameState_text.text = "게임\n준비";
            gameState_text.color = Color.green;
        }
        else
        {
            gameState_text.text = "게임중";
            gameState_text.color = Color.red;
        }

    }


    
}
