using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private StaticValue staticValue;

    private void Start()
    {
        staticValue = FindObjectOfType<StaticValue>();
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
