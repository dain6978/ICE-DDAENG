using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    private UIManager uiManager;
    PlayerController[] playerControllers;

    private float time;
    private float gameTime = 10f;

    [HideInInspector]
    public bool isEnd;

    
    private void Start()
    {
        AudioManager.Instacne.StopBGM();
        AudioManager.Instacne.PlayBGM("Game");

        StartGame();

        uiManager = FindObjectOfType<UIManager>();
    }

    private void StartGame()
    {
        time = 0;
        isEnd = false;
        Hashtable hash = new Hashtable();
        hash.Add("isEnd", isEnd);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    private void Update()
    {
        if (isEnd) return;

        time += Time.deltaTime;

        if (time > gameTime)
            EndGame();
    }


    private void EndGame()
    {
        isEnd = true;

        Hashtable hash = new Hashtable();
        hash.Add("isEnd", isEnd);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.IsLocal)
            {
                RoomManager.Instance.playerDict[player].playerController.GetComponent<PlayerController>().SetPlayersEnded();
                // Player UI ����
                //playerObject.GetComponentInChildren<PlayerUI>().Hide();
                // �� ���� (����ȭ ó�� �ؾ� ��)
                //playerObject.GetComponent<PlayerController>().GetItems().SetActive(false); 
                // �ִϸ��̼� ó�� (����ȭ ó�� �ؾ� ��)
                //playerObject.GetComponentInChildren<PlayerAnimManager>().SetDancingMode();            
            }
        }
        uiManager.DestroyGameUI();
        Destroy(uiManager);

        RankingManager.Instance.ShowRanking();

        Invoke(nameof(OnGameEnd), 60f);
    }


    private void OnGameEnd()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        RoomManager.roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.LeaveRoom();
        AudioManager.Instacne.PlayBGM("Lobby");
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // ����Ƽ ������ �÷��� ����  
#else
        Application.Quit(); // ���ø����̼� (���� ����) ����
#endif
    }


    //public void LeaveRoom()
    //{
    //    if (!isEnd)
    //    {
    //        Debug.Log("On Click Leave Room");
    //        PhotonNetwork.Disconnect();
    //        SceneManager.LoadScene(0);
    //        //
    //        // Disconnect: �� Ŭ���̾�Ʈ�� Photon �������� ���� ���� �մϴ�.���� ������ �Ϸ�� OnDisconnectedFromPhoton �� ȣ�� �˴ϴ�.
    //        // Disconnect ������ ��� LeaveRoom �� �ڵ����� ����� -> OnLeftRoom -> OnDisconnected
    //    }
    //}


}
