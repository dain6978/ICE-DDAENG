using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//ų, ����, �������� ������Ʈ ó���ϴ� ��ũ��Ʈ
//MonoBehaviourPunCallbacks�� ����ؾ�����
//custom property�� ������Ʈ�ɶ� callback ����������
public class Ranking_ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;
    public TMP_Text rankingText;

    Player player;

    //Player class�� ������ �÷��̾ ���õ� ���� ������ �� ����
    public void Initialize(Player player, int rank)
    {
        usernameText.text = player.NickName;
        rankingText.text = rank.ToString();
        this.player = player;
        UpdateStats();
    }

    //kills, deaths �Ӽ��� ui text�� �ݿ�
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
