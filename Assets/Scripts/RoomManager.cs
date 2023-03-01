using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO; //to access to the path class
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    //씬이 스위치될 때와 플레이어 매니저 프리팹이 인스턴스화될 때를 감지하기 위해 RoomManager를 싱글톤으로
    public static RoomManager Instance;

    private void Awake() //전형적인 싱글톤 패턴
    {
        if (Instance) //checks if another RoomManager exists
        {
            Destroy(gameObject); //there can only be one
            return;
        }
        DontDestroyOnLoad(gameObject); //if I am the only one
        Instance = this;
    }


    //Photon 클래스에서 상속받는 OnEnable과 OnDisable 함수는 base 함수를 호출해야 할 필요가 있음
    //왜지?? 다른 애들은 오히려 base를 호출하지 않고 재정의 하는 것이 안전한데 얘네는 필요하대)

    //OnEnable(): (스크립트나 오브젝트가) 활성화 될 때마다 호출되는 함수(Awake/Start와 달리 활성화 될 때마다)
    //OnDisable(): 비활성화 될 때마다 호출되는 함수
    public override void OnEnable()
    {
        base.OnEnable();
        //델리게이트 체인 추가 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //델리게이트 체인 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    //씬이 로드될 때마다 실행되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) //game scene
        {
            //게임 씬이 로드될 때마다 PhotonPrefabs 폴더 안에 PlayerManager를 생성 (프리팹의 위치 & 회전 0)
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }

    public void DestroyPlayer()
    {
        //PhotonNetwork.Destroy(playerManager);
    }

    // PhotonView를 가진 객체가 삭제되지 않게 사용한 DontDestoryOnLoad를 사용했을 때, 객체가 이동한 씬에 이미 있는 경우
    // 즉 RoomManager의 PhotonView가 중복되는 경우 발생하는 에러 방지하기 위해 먼저 객체를 삭제한 후 씬 이동
    //public void LoadMenuScene()
    //{
    //    Destroy(gameObject);
    //    SceneManager.LoadScene(0);
    //    //MenuManager.Instance.OpenMenu("loading");
    //    PhotonNetwork.LeaveRoom();
    //}

    public override void OnLeftRoom()
    {
        Debug.Log("On Left Room");
        // 게임 씬에서 방을 떠난 거라면
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);

            Debug.Log("방을 잘 떠났습니다. 나는 룸 매니저예요");
        }
    }

}
