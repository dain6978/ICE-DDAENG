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
public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;

    Player player;

    //Player class�� ������ �÷��̾ ���õ� ���� ������ �� ����
    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //CustomProperty Update�� targetPlayer�� �� player���,
        if(targetPlayer == player)
        {
            //Update�� �Ӽ��� kills �Ǵ� deaths���,
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
            {
                UpdateStats();
            }
        }
    }
}
