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
    GameObject playerController;

    int kills;
    int deaths;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        //photon view가 local player에 의해 소유된 경우 PV.IsMine이 true
        //playerManager 프리팹은 각각 다른 오너/플레이어 가짐 -> 우리 게임에선 인스턴스화된 각 플레이어 매니저에 대해 true
        {
            CreateController();
        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.Destroy(playerController); // 플레이어 매니저 파괴될 때 플레이어 컨트롤러도 파괴되도록
    }

    void CreateController()
    {
        // Instantiate our player controller
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
        playerController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        // PhotonNetwork.Instantiate(string prefabNames, Vector3 position, Quternion rotation, byte group = 0, object[] data = null) 
        // group: 0 is the group of prefab 
        // data : actual parameters that pass into the prefab we instantiate -> send the view id into the instantiation method
        // 플레이어 컨트롤러 생성할 때마다(플레이어 매니저 & 컨트롤러 1:1 관계) 각 플레이어 컨트롤러에 대한 viewID를 생성하겠다는 건가? 
    }

    public void Die()
    {
        PhotonNetwork.Destroy(playerController); // 죽으면 씬에 있는 playerController 파괴
        
        CreateController(); // 리스폰

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    
    //Kill한 클라이언트에게서만 호출되어야하는데
    //Kill당한 클라이언트에게서도 호출됨..
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
}
