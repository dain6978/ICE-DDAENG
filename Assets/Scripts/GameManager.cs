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
    private float time;
    private float gameTime = 10f;

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

        Debug.Log("게임 종료");

        RankingManager.Instance.ShowRanking();

        Invoke(nameof(OnGameEnd), 5f);
    }

    private void OnGameEnd()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        RoomManager.roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.Disconnect();
        //SceneManager.LoadScene(0);
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
            SceneManager.LoadScene(0);
            //
            // Disconnect: 이 클라이언트를 Photon 서버에서 접속 해제 합니다.룸을 나가고 완료시 OnDisconnectedFromPhoton 이 호출 됩니다.
            // Disconnect 실행할 경우 LeaveRoom 이 자동으로 실행됨 -> OnLeftRoom -> OnDisconnected
        }

    }
}
