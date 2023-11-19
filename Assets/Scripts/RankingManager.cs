using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO; 
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
    [SerializeField] GameObject rankingCamera;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject); 
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
            
            if (player == null) continue;

            if (PhotonNetwork.CurrentRoom.PlayerCount <= i)
                break;
            
            if (player.IsLocal)
                RoomManager.Instance.playerDict[player].playerController.GetComponent<PlayerController>().SetRanking(i);
            //Debug.Log(i + 1 + "µî: " + player.ToString());
            i++;
        }

        ShowScoreboard(rankingList);
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

    public void RankingCameraOn()
    {
        rankingCamera.SetActive(true);
    }

}
