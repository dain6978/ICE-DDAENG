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
        UnityEditor.EditorApplication.isPlaying = false;  // 유니티 에디터 플레이 종료  
#else
        Application.Quit(); // 어플리케이션 (빌드 파일) 종료
#endif
    }

    public void LeaveRoom()
    {
        staticValue.leaveGameRoom = true; 
        Debug.Log("On Click Leave Room");
        PhotonNetwork.Disconnect();
        // Disconnect: 이 클라이언트를 Photon 서버에서 접속 해제 합니다.룸을 나가고 완료시 OnDisconnectedFromPhoton 이 호출 됩니다.
        // Disconnect 실행할 경우 LeaveRoom 이 자동으로 실행됨 -> OnLeftRoom -> OnDisconnected
    }
}
