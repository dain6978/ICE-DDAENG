using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Linq;
using Photon.Realtime;
//기존 Hasytable 클래스가 아닌, Photon Hashtable 클래스를 사용할 거라 선언
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    // 플레이어의 death와 respawning 관리

    PhotonView PV;
    public GameObject playerController;
    SpawnManager spawnManager;
    UIManager uiManager;
    DancingManager dancingManager;

    int kills;
    int deaths;

    int skinIndex;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        dancingManager = FindObjectOfType<DancingManager>();
    }

    void Start()
    {
        if (PV.IsMine)
        //photon view가 local player에 의해 소유된 경우 PV.IsMine이 true
        //playerManager 프리팹은 각각 다른 오너/플레이어 가짐 -> 우리 게임에선 인스턴스화된 각 플레이어 매니저에 대해 true
        {
            CreateController();
            skinIndex = PlayerPrefs.GetInt("userskin");
            Hashtable hash = new Hashtable();
            hash.Add("kills", 0);
            hash.Add("deaths", 0);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        RoomManager.Instance.playerDict.Add(PV.Owner, this);
    }


    void CreateController()
    {
        // Instantiate our player controller
        Transform spawnpoint = spawnManager.GetSpawnPoint();
        playerController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });

        playerController.GetComponent<PlayerController>().SetCharacterSkin();
        // 플레이어 컨트롤러 생성할 때마다(플레이어 매니저 & 컨트롤러 1:1 관계) 각 플레이어 컨트롤러에 대한 viewID를 생성하겠다는 건가? 

        //uiManager.SetPlayer(playerController);
        //dancingManager.SetPlayer(playerController);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(playerController); // 죽으면 씬에 있는 playerController 파괴
        playerController = null;
        //uiManager.SetPlayer(null);
        //dancingManager.SetPlayer(null);


        CreateController(); // 리스폰

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    
    public void GetKill()
    {
        PV.RPC(nameof(RPC_GetKill), PV.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public static PlayerManager Find(Player player)
    {
        //씬의 모든 플레이어매니저 배열 리턴
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }


    private void OnDisable()
    {
        if (PV.IsMine)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //Debug.Log("커서 잠금 해제");
        }
    }

}
