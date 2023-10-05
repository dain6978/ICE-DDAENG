using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO; //to access to the path class
using Photon.Realtime;
using System.Linq;

public class RankingManager : MonoBehaviourPunCallbacks
{
    public static RankingManager Instance;
    public Dictionary<Player, int> rankingDict = new Dictionary<Player, int>();
    public Transform[] rankingPoints;

    [SerializeField] Transform container;
    [SerializeField] GameObject ranking_scoreboardItemPrefab;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject scoreboardTitle;

    //Dictionary로 관리
    //Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();

    private void Awake() //전형적인 싱글톤 패턴
    {
        if (Instance) //checks if another RoomManager exists
        {
            Destroy(gameObject); //there can only be one
            return;
        }
        Instance = this;
    }

    
    public void ShowRanking()
    {
        Dictionary<Player, int> scoreboardDict = new Dictionary<Player, int>();
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            scoreboardDict.Add(player, (int)(player.CustomProperties["kills"]));
        }

        var sortVar = from item in scoreboardDict
                    orderby item.Value descending
                    select item;

        rankingDict = sortVar.ToDictionary(x => x.Key, x => x.Value);

        List<Player> rankingList = rankingDict.Keys.ToList();

        int i = 0;
        foreach(Player player in rankingList)
        {
            Debug.Log(i+1 + "등: "+ player.ToString());
            if (player == null) continue;

            if (PhotonNetwork.CurrentRoom.PlayerCount <= i)
                break;
            
            if (player.IsLocal)
                RoomManager.Instance.playerDict[player].playerController.GetComponent<PlayerController>().SetRanking(i);
            
            i++;
        }

        ShowScoreboard(rankingList);
        //Invoke("OnGameEnd", 10f);

    }
    private void OnGameEnd()
    {
        Debug.Log("Hi");
        PhotonNetwork.CurrentRoom.IsOpen = true;
        RoomManager.roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.Disconnect();
    }

    private void ShowScoreboard(List<Player> rankingList)
    {
        int rank = 1;
        foreach (Player player in rankingList)
        {
            if (player == null) continue;

            Ranking_ScoreboardItem item = Instantiate(ranking_scoreboardItemPrefab, container).GetComponent<Ranking_ScoreboardItem>();
            item.Initialize(player, rank);
            rank++;
        }
        canvasGroup.alpha = 1;
        scoreboardTitle.SetActive(true);
    }

}
