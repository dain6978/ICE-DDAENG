using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

//���� ���񽺿� �����ϰ�, ������ ����(��Ʈ��ũ ���� �ٸ� ���ӵ�� �ٸ� �÷��̾���� ã�� �� �ְ� �ϴ�)�� �����ϴ� ��ũ��Ʈ

public class Launcher : MonoBehaviourPunCallbacks
//MonoBehavior���� MonoBehaviourPunCallbacks�� Ŭ������ Ȯ�������ν�, �� ����, ���� ����, �κ� ���� �� ���� ���� ���ῡ �ʿ��� �ݹ鿡 ������ �� �ִ�.
{

    public static Launcher Instance; //�̱���

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
        //'PhotonServerSettings'�� ������ Ȱ���Ͽ� ���� ������ ������ ������ �� �ְ� ��
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }


    //OnConnectedToMaster: ������ ������ ���������� ����Ǿ��� �� ���濡 ���� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        //�κ�� �⺻������ ���� �����ϰ�, ã�� �� �ִ� ��
        PhotonNetwork.AutomaticallySyncScene = true;
        //AutomaticallySyncScene ȣ��Ʈ(������)�� ���� �ε��� �� ��� Ŭ���̾�Ʈ�� �ڵ����� ���ÿ� �ε����� ���� ����
    }

    //�κ� ����� ��� ����Ǵ� �Լ� (OnConnectedToMaster -> PhotonNetwork.JoinLobby -> OnJoinedLobby)
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
        {//���� �Է��� �� ������ �����̸�
            return; //���� x
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading"); //�÷��̾ ����� ��ǲ�ʵ带 �Է��ϰ� ������ ���� ������ ������ loading ��� �ٸ� Ŭ�� ����
    }

    //���������� ������ �뿡 ���ԵǾ��� �� ����Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        isFirstConnection = false;
        MenuManager.Instance.OpenMenu("room");
        roomText.text = PhotonNetwork.CurrentRoom.Name;


        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
            //�뿡 ������ ������ PlayerListContent�� �ڽĵ� (�÷��̾��) �ʱ�ȭ
        }

        Player[] players = PhotonNetwork.PlayerList;

        //�÷��̾ ó�� �뿡 ���� ��, �ش� �뿡 �������ִ� �÷��̾�� �̸� ��� ǥ��
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient); //���� ������ Ŭ���̾�Ʈ��� ��ŸƮ��ư Ȱ��ȭ (ȣ��Ʈ�� ���� ������ �� �ְ�)
    }

    //������ Ŭ���̾�Ʈ�� ��ü�� �� ����Ǵ� �Լ�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); 
    }

    //������ �뿡 ������ �� �������� �� ����Ǵ� �ݹ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
        //�׸��� OnClick �Լ��� ���� ok ��ư ������ title �޴��� ���ư��� 
    }

    public void LeaveRoom()
    {
        //Leave Room ��ư�� Ŭ���ϸ�, ������ ���� �����ڴٴ� ����� ������ ������ ���� loading ���
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        //leave room ����� �Ϸ�Ǹ� room setting �޴���
        MenuManager.Instance.OpenMenu("room setting");
        // LeaveRoom -> PhotonNetwork.LeaveRoom -> OnLeftRoom
        // PhotonNetwork.LeaveRoom �Լ����� ������ ������ ���ư��� ������ OnConnectedToMaster�� ȣ���
        // OnConnectedToMaster���� �ٷ� return�� ��� �κ� ���� ���� �߻� -> �������� �״�� �����ϵ� open�ϴ� menu�� �ٸ��� ����
    }

    // RoomListItemButton�� Ŭ���ؼ� �뿡 �����ϸ�, ��Ʈ��ũ�� ��� & �뿡 �����ϴ� ���� �ε� �޴�
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //roomList ������ room information Ŭ������ RoomInfo�� Ŭ������ ���� Ŭ����
        //���� �̸�, �ִ� ���� �� ���� �Ӽ� 
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
            //�� ����Ʈ�� ������Ʈ �� ������, roomListContent�� ���ӿ�����Ʈ����(roomListItem) �ı��ؼ� ����Ʈ ���� �� ����� (������Ʈ)
        }

        for (int i=0; i<roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue; //�� ����Ʈ �������� ������ ��� continue, �� �ش� ������ ���� X, �� �� ����Ʈ���� �ٷ� ����
            }
            //Instantiate()�Լ�: ������ �����ϴ� ���߿� ���ӿ�����Ʈ ����
            //roomListContent �����̳� �ȿ� �ִ� rooListItemPrefab���� RoomListItem ��ũ��Ʈ�� �����ͼ�, Setup�Լ� ȣ��
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //���ο� �ٸ� (�ڱ��ڽ��� JoinRoom����) �÷��̾ ������ playerListContent�� �ڽ����� �÷��̾��Ʈ������ ������ �����ϰ� ���ο��÷��̾�� ����
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


    public void LoadGameScene()
    {
        PhotonNetwork.LoadLevel(1);
        //'Game' �� �ε� (���� ���ÿ��� Game ���� �ε����� 1�� �����߱� ������)
        // ����Ƽ SceneManager�� �� ��ü ��� ���� ��Ʈ��ũ�� LoadLevel�� ����ϴ� ����
        // : ȣ��Ʈ�� Ư�� �÷��̾ �� ��ü �Լ��� ȣ���ϴ� ���, ������(�ش� ����) ��� �÷��̾ �� ���� ������ �ε��ϵ��� �ϱ� ����
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // ����Ƽ ������ �÷��� ����  
#else
        Application.Quit(); // ���ø����̼� (���� ����) ����
#endif
    }
}