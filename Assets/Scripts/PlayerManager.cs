using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Linq;
using Photon.Realtime;
//���� Hasytable Ŭ������ �ƴ�, Photon Hashtable Ŭ������ ����� �Ŷ� ����
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerManager : MonoBehaviour
{
    // �÷��̾��� death�� respawning ����

    PhotonView PV;
    GameObject playerController;
    SpawnManager spawnManager;
    //GameManager gameManager;

    int kills;
    int deaths;

    int skinIndex;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Start()
    {
        if (PV.IsMine)
        //photon view�� local player�� ���� ������ ��� PV.IsMine�� true
        //playerManager �������� ���� �ٸ� ����/�÷��̾� ���� -> �츮 ���ӿ��� �ν��Ͻ�ȭ�� �� �÷��̾� �Ŵ����� ���� true
        {
            CreateController();
            skinIndex = PlayerPrefs.GetInt("userskin");
            Hashtable hash = new Hashtable();
            hash.Add("kills", 0);
            hash.Add("deaths", 0);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
    }

    void CreateController()
    {
        // Instantiate our player controller
        Transform spawnpoint = spawnManager.GetSpawnPoint();
        playerController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });

        playerController.GetComponent<PlayerController>().SetCharacterSkin();
        // �÷��̾� ��Ʈ�ѷ� ������ ������(�÷��̾� �Ŵ��� & ��Ʈ�ѷ� 1:1 ����) �� �÷��̾� ��Ʈ�ѷ��� ���� viewID�� �����ϰڴٴ� �ǰ�? 
    }

    public void Die()
    {
        PhotonNetwork.Destroy(playerController); // ������ ���� �ִ� playerController �ı�
        
        CreateController(); // ������

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    
    //Kill�� Ŭ���̾�Ʈ���Լ��� ȣ��Ǿ���ϴµ�
    //Kill���� Ŭ���̾�Ʈ���Լ��� ȣ���..
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
        //���� ��� �÷��̾�Ŵ��� �迭 ����
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }

    public GameObject GetPlayerController()
    {
        if (playerController != null)
        {
            return playerController;
        }
        else
        {
            return null;
        }
    }

}
