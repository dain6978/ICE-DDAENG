using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

//MonoBehaviour�� ����ϵ��� �ϸ� .. �ڱⰡ ���������� ������
//� �÷��̾�� ������ ������Ʈ �ǰ� ����� �ϹǷ�
//MonoBehaviourPunCallbacks ���
public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
    [SerializeField] CanvasGroup canvasGroup;

    //Dictionary�� ����
    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();
    private void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            //�ڱⰡ �濡 �������� ������Ʈ
            AddScoreboardItem(player);
        }
    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //������ �濡 �������� ������Ʈ
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    void AddScoreboardItem(Player player)
    {
        //�����̳ʿ��ٰ� ������ �� scoreboardItemPrefab�� ����
        ScoreboardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        item.Initialize(player);

        //Dictionary���� ����ֱ�
        scoreboardItems[player] = item;
    }

    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    private void Update()
    {
        //ų���� ���� �ѱ�(���� ����)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvasGroup.alpha = 1;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            canvasGroup.alpha = 0;
        }
    }
}
