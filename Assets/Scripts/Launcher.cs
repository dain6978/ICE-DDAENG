using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

//포톤 서비스에 연결하고, 마스터 서버(네트워크 상의 다른 게임들과 다른 플레이어들을 찾을 수 있게 하는)에 연결하는 스크립트

public class Launcher : MonoBehaviourPunCallbacks
//MonoBehavior에서 MonoBehaviourPunCallbacks로 클래스를 확장함으로써, 룸 생성, 오류 제어, 로비에 가입 등 포톤 서버 연결에 필요한 콜백에 접근할 수 있다.
{

    public static Launcher Instance; //싱글톤

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomText;
    
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    bool isFirstConnection = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //'PhotonServerSettings'의 설정을 활용하여 포톤 마스터 서버에 접근할 수 있게 함
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }


    //OnConnectedToMaster: 마스터 서버에 성공적으로 연결되었을 때 포톤에 의해 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        //로비란 기본적으로 룸을 생성하고, 찾을 수 있는 곳
        PhotonNetwork.AutomaticallySyncScene = true;
        //AutomaticallySyncScene 호스트(마스터)가 씬을 로드할 때 모든 클라이언트가 자동으로 동시에 로드할지 말지 결정
    }

    //로비에 연결될 경우 실행되는 함수 (OnConnectedToMaster -> PhotonNetwork.JoinLobby -> OnJoinedLobby)
    public override void OnJoinedLobby()
    {
        if (isFirstConnection)
        {
            MenuManager.Instance.OpenMenu("title");
        }
        else
        {
            MenuManager.Instance.OpenMenu("room setting");
        }
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {//만약 입력한 룸 네임이 공백이면
            return; //생성 x
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading"); //플레이어가 룸네임 인풋필드를 입력하고 서버에 룸이 생성될 때까지 loading 띄워 다른 클릭 방지
    }

    //성공적으로 생성된 룸에 가입되었을 때 실행되는 콜백 함수
    public override void OnJoinedRoom()
    {
        isFirstConnection = false;
        MenuManager.Instance.OpenMenu("room");
        roomText.text = PhotonNetwork.CurrentRoom.Name;


        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
            //룸에 가입할 때마다 PlayerListContent의 자식들 (플레이어들) 초기화
        }

        Player[] players = PhotonNetwork.PlayerList;

        //플레이어가 처음 룸에 들어갔을 때, 해당 룸에 접속해있는 플레이어들 이름 모두 표시
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient); //만약 마스터 클라이언트라면 스타트버튼 활성화 (호스트만 게임 시작할 수 있게)
    }

    //마스터 클라이언트가 교체될 때 실행되는 함수
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); 
    }

    //생성된 룸에 가입한 걸 실패했을 때 실행되는 콜백 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
        //그리고 OnClick 함수를 통해 ok 버튼 누르면 title 메뉴로 돌아가게 
    }

    public void LeaveRoom()
    {
        //Leave Room 버튼을 클릭하면, 서버에 룸을 떠나겠다는 명령을 내리고 떠나는 동안 loading 출력
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        //leave room 명령이 완료되면 room setting 메뉴로
        MenuManager.Instance.OpenMenu("room setting");
        // LeaveRoom -> PhotonNetwork.LeaveRoom -> OnLeftRoom
        // PhotonNetwork.LeaveRoom 함수에서 마스터 서버로 돌아가기 때문에 OnConnectedToMaster가 호출됨
        // OnConnectedToMaster에서 바로 return할 경우 로비 관련 문제 발생 -> 나머지는 그대로 유지하되 open하는 menu만 다르게 설정
    }

    // RoomListItemButton을 클릭해서 룸에 가입하면, 네트워크에 명령 & 룸에 가입하는 동안 로딩 메뉴
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //roomList 포톤의 room information 클래스인 RoomInfo의 클래스에 대한 클래스
        //룸의 이름, 최대 유저 수 등의 속성 
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
            //룸 리스트가 업데이트 될 때마다, roomListContent의 게임오브젝트들을(roomListItem) 파괴해서 리스트 삭제 후 재생성 (업데이트)
        }

        for (int i=0; i<roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue; //룸 리스트 아이템이 삭제될 경우 continue, 즉 해당 프리팹 생성 X, 즉 룸 리스트에서 바로 삭제
            }
            //Instantiate()함수: 게임을 실행하는 도중에 게임오브젝트 생성
            //roomListContent 컨테이너 안에 있는 rooListItemPrefab에서 RoomListItem 스크립트를 가져와서, Setup함수 호출
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //새로운 다른 (자기자신은 JoinRoom에서) 플레이어가 들어오면 playerListContent의 자식으로 플레이어리스트아이템 프리팹 생성하고 새로운플레이어로 세팅
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


    public void LoadGameScene()
    {
        PhotonNetwork.LoadLevel(1);
        //'Game' 씬 로드 (빌드 세팅에서 Game 씬의 인덱스를 1로 설정했기 때문에)
        // 유니티 SceneManager의 씬 교체 대신 포톤 네트워크의 LoadLevel을 사용하는 이유
        // : 호스트나 특정 플레이어가 씬 교체 함수를 호출하는 대신, 게임의(해당 룸의) 모든 플레이어가 한 번에 레벨을 로드하도록 하기 위해
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // 유니티 에디터 플레이 종료  
#else
        Application.Quit(); // 어플리케이션 (빌드 파일) 종료
#endif
    }
}