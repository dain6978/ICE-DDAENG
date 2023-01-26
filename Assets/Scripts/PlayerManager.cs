using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    // �÷��̾��� death�� respawning ����

    PhotonView PV;
    GameObject playerController;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        //photon view�� local player�� ���� ������ ��� PV.IsMine�� true
        //playerManager �������� ���� �ٸ� ����/�÷��̾� ���� -> �츮 ���ӿ��� �ν��Ͻ�ȭ�� �� �÷��̾� �Ŵ����� ���� true
        {
            CreateController();
        }
    }

    void CreateController()
    {
        // Instantiate our player controller
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
        playerController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        // PhotonNetwork.Instantiate(string prefabNames, Vector3 position, Quternion rotation, byte group = 0, object[] data = null) 
        // group: 0 is the group of prefab 
        // data : actual parameters that pass into the prefab we instantiate -> send the view id into the instantiation method
        // �÷��̾� ��Ʈ�ѷ� ������ ������(�÷��̾� �Ŵ��� & ��Ʈ�ѷ� 1:1 ����) �� �÷��̾� ��Ʈ�ѷ��� ���� viewID�� �����ϰڴٴ� �ǰ�? 
    }

    public void Die()
    {
        PhotonNetwork.Destroy(playerController); // ������ ���� �ִ� playerController �ı�
        
        CreateController(); // ������
    }
    
}
