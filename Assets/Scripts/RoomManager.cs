using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO; //to access to the path class

public class RoomManager : MonoBehaviourPunCallbacks
{
    //���� ����ġ�� ���� �÷��̾� �Ŵ��� �������� �ν��Ͻ�ȭ�� ���� �����ϱ� ���� RoomManager�� �̱�������
    public static RoomManager Instance;

    private void Awake() //�������� �̱��� ����
    {
        if (Instance) //checks if another RoomManager exists
        {
            Destroy(gameObject); //there can only be one
            return;
        }
        DontDestroyOnLoad(gameObject); //if I am the only one
        Instance = this;
    }


    [HideInInspector] public GameObject player;

    //Photon Ŭ�������� ��ӹ޴� OnEnable�� OnDisable �Լ��� base �Լ��� ȣ���ؾ� �� �ʿ䰡 ����
    //����?? �ٸ� �ֵ��� ������ base�� ȣ������ �ʰ� ������ �ϴ� ���� �����ѵ� ��״� �ʿ��ϴ�)

    //OnEnable(): (��ũ��Ʈ�� ������Ʈ��) Ȱ��ȭ �� ������ ȣ��Ǵ� �Լ�(Awake/Start�� �޸� Ȱ��ȭ �� ������)
    //OnDisable(): ��Ȱ��ȭ �� ������ ȣ��Ǵ� �Լ�
    public override void OnEnable()
    {
        base.OnEnable();
        //��������Ʈ ü�� �߰� 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //��������Ʈ ü�� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    //���� �ε�� ������ ����Ǵ� �Լ�
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) //game scene
        {
            //���� ���� �ε�� ������ PhotonPrefabs ���� �ȿ� PlayerManager�� ���� (�������� ��ġ & ȸ�� 0)
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }

        else if (scene.buildIndex == 0) //menu scene
        {
            //PhotonNetwork.Destroy(playerManager);
            Debug.Log("���� �Ŵ��� - �޴� �� �ε�"); // �ϴ� ��� �׻� ������ ��
        }
    }
}
