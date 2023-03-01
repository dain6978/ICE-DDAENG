using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{

    private RoomManager roomManager;
    private PlayerManager playerManager;


    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        playerManager = FindObjectOfType<PlayerManager>();

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
        Debug.Log("On Click Leave Room");

        PhotonNetwork.LeaveRoom();
    }

    //public override void OnLeftRoom()
    //{
    //    PhotonNetwork.LoadLevel(0);
    //}
}
