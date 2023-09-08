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
        
        Debug.Log("게임 종료");
        Player winner = PhotonNetwork.LocalPlayer;  //위너가 없으면.. 일단 로컬플레이어로 초기화 하겠음 ㅎ
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
        UnityEditor.EditorApplication.isPlaying = false;  // 유니티 에디터 플레이 종료  
#else
        Application.Quit(); // 어플리케이션 (빌드 파일) 종료
#endif
    }

    
    public void LeaveRoom()
    {
        if (!isEnd)
        {
            Debug.Log("On Click Leave Room");
            PhotonNetwork.Disconnect();
            // Disconnect: 이 클라이언트를 Photon 서버에서 접속 해제 합니다.룸을 나가고 완료시 OnDisconnectedFromPhoton 이 호출 됩니다.
            // Disconnect 실행할 경우 LeaveRoom 이 자동으로 실행됨 -> OnLeftRoom -> OnDisconnected
        }

    }
}
