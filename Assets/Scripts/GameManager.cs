using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private StaticValue staticValue;
    private float time;
    private float gameTime = 300f;
    bool isEnd;
    private void Start()
    {
        staticValue = FindObjectOfType<StaticValue>();
        StartGame();
        
    }

    private void StartGame()
    {
        time = 0;
        isEnd = false;
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
        Debug.Log("���� ����");
        Invoke("OnGameEnd", 5f);
    }

    private void OnGameEnd()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        staticValue.roomName = PhotonNetwork.CurrentRoom.Name;
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
        
        staticValue.leaveGameRoom = true; 
        Debug.Log("On Click Leave Room");
        PhotonNetwork.Disconnect();
        // Disconnect: �� Ŭ���̾�Ʈ�� Photon �������� ���� ���� �մϴ�.���� ������ �Ϸ�� OnDisconnectedFromPhoton �� ȣ�� �˴ϴ�.
        // Disconnect ������ ��� LeaveRoom �� �ڵ����� ����� -> OnLeftRoom -> OnDisconnected
    }
}
