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
    private void Awake() //¿¸«¸¿˚¿Œ ΩÃ±€≈Ê ∆–≈œ
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
            if (player == null) continue;

            if (PhotonNetwork.CurrentRoom.PlayerCount <= i)
                break;

            if (i < 3)
            {
                RoomManager.Instance.playerDict[player].GetComponent<PlayerManager>().playerController.transform.position = rankingPoints[i].position;
                RoomManager.Instance.playerDict[player].GetComponent<PlayerManager>().playerController.GetComponent<PlayerController>().isEnd = false;
            }
            else
            {
                RoomManager.Instance.playerDict[player].GetComponent<PlayerManager>().playerController.transform.position = rankingPoints[3].position;
            }
        }

        //Invoke("OnGameEnd", 10f);

    }
    private void OnGameEnd()
    {
        Debug.Log("Hi");
        PhotonNetwork.CurrentRoom.IsOpen = true;
        RoomManager.roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.Disconnect();
    }

}
