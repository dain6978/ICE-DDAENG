using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//�÷��̾ �뿡 �����ϸ�, ��ó�� �ݹ��� �ް�, �÷��̾��Ʈ������ �������� �����ϰ�, �÷��̾ ���� ������ �÷��̾��Ʈ������������ ����
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
            //�ٸ� �÷��̾ ���� ������ destroy?? �� �𸣰� �򰥸�
        }
    }

    public override void OnLeftRoom()
    {
        //�÷��̾ (��?)�� ���� ������ destroy??
        Destroy(gameObject);
    }
}
