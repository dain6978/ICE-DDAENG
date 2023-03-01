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
        UnityEditor.EditorApplication.isPlaying = false;  // ����Ƽ ������ �÷��� ����  
#else
        Application.Quit(); // ���ø����̼� (���� ����) ����
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
