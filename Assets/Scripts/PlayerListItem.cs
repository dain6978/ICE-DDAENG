using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//플레이어가 룸에 가입하면, 런처가 콜백을 받고, 플레이어리스트아이템 프리팹을 생성하고, 플레이어가 룸을 떠나면 플레이어리스트아이템프리팹 제거
public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    

    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }



    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
            //다른 플레이어가 방을 떠나면 destroy?? 잘 모르겠 헷갈림
        }
    }

    public override void OnLeftRoom()
    {
        //플레이어가 (나?)가 방을 떠나도 destroy??
        Destroy(gameObject);
    }
}
