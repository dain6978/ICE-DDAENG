using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    private float time;
    private float gameTime = 30f;

    bool isEnd;

    
    private void Start()
    {
        StartGame();
        
    }

    private void StartGame()
    {
        time = 0;
        isEnd = false;
        Hashtable hash = new Hashtable();
        hash.Add("isEnd", false);
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
        hash.Add("isEnd", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        
        Debug.Log("���� ����");
        Player winner = PhotonNetwork.LocalPlayer;  //���ʰ� ������.. �ϴ� �����÷��̾�� �ʱ�ȭ �ϰ��� ��
        int mostKillCount = 0;

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if ((int)(player.CustomProperties["kills"]) > mostKillCount)
            {
                mostKillCount = (int)player.CustomProperties["kills"];
                winner = player;
            }
        }

        Debug.Log($"Winner: {winner}, Kill: {mostKillCount}");
        Invoke("OnGameEnd", 5f);
    }

    private void OnGameEnd()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        RoomManager.roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.Disconnect();        
    }
    
    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // ����Ƽ ������ �÷��� ����  
#else
        Application.Quit(); // ���ø����̼� (���� ����) ����
#endif
    }

    
    public void LeaveRoom()
    {
        if (!isEnd)
        {
            Debug.Log("On Click Leave Room");
            PhotonNetwork.Disconnect();
            // Disconnect: �� Ŭ���̾�Ʈ�� Photon �������� ���� ���� �մϴ�.���� ������ �Ϸ�� OnDisconnectedFromPhoton �� ȣ�� �˴ϴ�.
            // Disconnect ������ ��� LeaveRoom �� �ڵ����� ����� -> OnLeftRoom -> OnDisconnected
        }

    }
}
