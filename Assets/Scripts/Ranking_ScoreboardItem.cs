using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//킬, 데쓰, 유저네임 업데이트 처리하는 스크립트
//MonoBehaviourPunCallbacks를 상속해야지만
//custom property가 업데이트될때 callback 받을수있음
public class Ranking_ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;
    public TMP_Text rankingText;

    Player player;

    //Player class는 포톤이 플레이어에 관련된 모든걸 추적할 때 사용됨
    public void Initialize(Player player, int rank)
    {
        usernameText.text = player.NickName;
        rankingText.text = rank.ToString();
        this.player = player;
        UpdateStats();
    }

    //kills, deaths 속성을 ui text에 반영
    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("kills", out object kills)){
            killsText.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            deathsText.text = deaths.ToString();
        }
    }
}
