using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

//MonoBehaviour을 상속하도록 하면 .. 자기가 들어왔을때만 생성됨
//어떤 플레이어든 들어오면 업데이트 되게 해줘야 하므로
//MonoBehaviourPunCallbacks 상속
public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
    [SerializeField] CanvasGroup canvasGroup;

    //Dictionary로 관리
    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();
    private void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            //자기가 방에 들어왔을때 업데이트
            AddScoreboardItem(player);
        }
    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //누군가 방에 들어왓을때 업데이트
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    void AddScoreboardItem(Player player)
    {
        //컨테이너에다가 실제로 새 scoreboardItemPrefab을 생성
        ScoreboardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        item.Initialize(player);

        //Dictionary에다 집어넣기
        scoreboardItems[player] = item;
    }

    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    private void Update()
    {
        //킬데쓰 끄고 켜기(투명도 조절)
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
