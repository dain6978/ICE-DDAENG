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
    private DancingManager dancingManager;
    private UIManager uiManager;

    private float time;
    private float gameTime = 5f;

    [HideInInspector]
    public bool isEnd;

    
    private void Start()
    {
        StartGame();

        dancingManager = FindObjectOfType<DancingManager>();
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

        Debug.Log("���� ����");

        SetPlayers();
        RankingManager.Instance.ShowRanking();


        //player.AddComponent<DancingAnimation>();

        Invoke(nameof(OnGameEnd), 10f);
    }


    private void OnGameEnd()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        RoomManager.roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.LeaveRoom();
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // ����Ƽ ������ �÷��� ����  
#else
        Application.Quit(); // ���ø����̼� (���� ����) ����
#endif
    }

    public void SetPlayers()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player != null)
            {
                dancingManager.SetAnimator(player);
                //uiManager.SetUI(player);
            }
            else
            {
                Debug.Log("player is null");
            }
        }
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
