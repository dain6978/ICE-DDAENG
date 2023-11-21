using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

//포톤 서비스에 연결하고, 마스터 서버(네트워크 상의 다른 게임들과 다른 플레이어들을 찾을 수 있게 하는)에 연결하는 스크립트

public class Launcher : MonoBehaviourPunCallbacks
//MonoBehavior에서 MonoBehaviourPunCallbacks로 클래스를 확장함으로써, 룸 생성, 오류 제어, 로비에 가입 등 포톤 서버 연결에 필요한 콜백에 접근할 수 있다.
{
    public static Launcher Instance; //싱글톤

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomText;
    [SerializeField] TMP_Text noticeText;

    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject noticeWindow;

    Dictionary<RoomInfo, GameObject> roomdict;

    bool isFirstConnection = true;

    int minPlayers = 2;
    int maxPlayers = 8;
    //StaticValue staticValue;

    private void Awake()
    {
        Instance = this;
        roomdict = new Dictionary<RoomInfo, GameObject>();
        Cursor.visible = true;
    }

    void Start()
    {

        MenuManager.Instance.OpenMenu("loading");
        //'PhotonServerSettings'의 설정을 활용하여 포톤 마스터 서버에 접근할 수 있게 함
        //Debug.Log("Connecting to Master");

        PhotonNetwork.ConnectUsingSettings();

    }


    //OnConnectedToMaster: 마스터 서버에 성공적으로 연결되었을 때 포톤에 의해 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        //Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        //로비란 기본적으로 룸을 생성하고, 찾을 수 있는 곳
        PhotonNetwork.AutomaticallySyncScene = true;
        //AutomaticallySyncScene 호스트(마스터)가 씬을 로드할 때 모든 클라이언트가 자동으로 동시에 로드할지 말지 결정
    }

    //로비에 연결될 경우 실행되는 함수 (OnConnectedToMaster -> PhotonNetwork.JoinLobby -> OnJoinedLobby)
    public override void OnJoinedLobby()
    {
        //Debug.Log("Joined Lobby");
        //Debug.Log("RoomName" + RoomManager.roomName);
        if (RoomManager.roomName != null)
        {
            ReturnRoom();

            return;
        }

        AudioManager.Instacne.PlayBGM("Lobby");
        MenuManager.Instance.OpenMenu("title");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {//만약 입력한 룸 네임이 공백이면
            return; //생성 x
        }

        foreach(RoomInfo roomInfo in roomdict.Keys)
        {
            if(roomInfo.Name == roomNameInputField.text)
            {
                noticeText.text = "이미 존재하는 방입니다.";
                noticeWindow.SetActive(true);
                return;
            }
        }
        

        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = (byte)maxPlayers;
        ro.IsOpen = true;


        PhotonNetwork.CreateRoom(roomNameInputField.text, ro);
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
        // 메뉴 씬에서 방을 떠난 거라면
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            //leave room 명령이 완료되면 room setting 메뉴로
            MenuManager.Instance.OpenMenu("room setting");
            // LeaveRoom -> PhotonNetwork.LeaveRoom -> OnLeftRoom
            // PhotonNetwork.LeaveRoom 함수에서 마스터 서버로 돌아가기 때문에 OnConnectedToMaster가 호출됨
            // OnConnectedToMaster에서 바로 return할 경우 로비 관련 문제 발생 -> 나머지는 그대로 유지하되 open하는 menu만 다르게 설정
        }
    }

    // RoomListItemButton을 클릭해서 룸에 가입하면, 네트워크에 명령 & 룸에 가입하는 동안 로딩 메뉴
    public void JoinRoom(RoomInfo info)
    {
        if (!info.IsOpen)
        {
            noticeText.text = "이미 게임이 시작된 방입니다.";
            noticeWindow.SetActive(true);
            return;
        }
        else if(info.PlayerCount == info.MaxPlayers)
        {
            noticeText.text = "방이 꽉 찼습니다..";
            noticeWindow.SetActive(true);
            return;
        }

        PhotonNetwork.JoinRoom(info.Name);
        
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //roomList 포톤의 room information 클래스인 RoomInfo의 클래스에 대한 클래스
        //룸의 이름, 최대 유저 수, 오픈 여부 등의 속성 
        
        for (int i=0; i<roomList.Count; i++)
        {
            //삭제된 경우
            if (roomList[i].RemovedFromList)    
            {
                if (roomdict.Count == 0)
                    return;

                Destroy(roomdict[roomList[i]]);
                roomdict.Remove(roomList[i]);
                continue; 
            }

            //추가된 경우
            if (!roomdict.ContainsKey(roomList[i]))     
                roomdict.Add((roomList[i]), Instantiate(roomListItemPrefab, roomListContent));

            //변경된 경우
            roomdict[roomList[i]].GetComponent<RoomListItem>().SetUp(roomList[i]);      
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //새로운 다른 (자기자신은 JoinRoom에서) 플레이어가 들어오면 playerListContent의 자식으로 플레이어리스트아이템 프리팹 생성하고 새로운플레이어로 세팅
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


    public void LoadGameScene()
    {
        //if(PhotonNetwork.CurrentRoom.PlayerCount < minPlayers)
        //{
        //    Debug.Log($"인원 {minPlayers}명부터 게임 시작이 가능합니다.");
        //    return;
        //}
        
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(1);
        //'Game' 씬 로드 (빌드 세팅에서 Game 씬의 인덱스를 1로 설정했기 때문에)
        // 유니티 SceneManager의 씬 교체 대신 포톤 네트워크의 LoadLevel을 사용하는 이유
        // : 호스트나 특정 플레이어가 씬 교체 함수를 호출하는 대신, 게임의(해당 룸의) 모든 플레이어가 한 번에 레벨을 로드하도록 하기 위해
    }

    public void ReturnRoom()
    { 
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = (byte)maxPlayers;
        ro.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(RoomManager.roomName, ro, null);
        MenuManager.Instance.OpenMenu("room");
        roomText.text = RoomManager.roomName;

        foreach (Transform child in playerListContent)
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
        RoomManager.roomName = null;
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