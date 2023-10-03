using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
            //HasKey: 해당 키의 존재 여부 bool 값으로 반환
        {
            usernameInput.text = PlayerPrefs.GetString("username"); 
            //username 키가 존재하면, 즉 이미 인풋필드에서 유저 이름을 세팅한 데이터가 있다면, 해당 데이터를 포톤 네트워크의 닉네임으로
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        { // 기존 데이터 없으면 없으면 랜덤 이름으로
            usernameInput.text = "Player" + Random.Range(0, 1000).ToString("0000");
            OnUsernameInputValueChanged();
        }
    }
    
    //인풋 필드의 텍스트가 변화하 때마다 호출되는 함수
    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("username", usernameInput.text); // SetString: 지정한 키로 string 타입의 값 저장
        //PlayerPrefs: 유니티에서 제공하는 데이터 관리 클래스로, 데이터의 저장 및 불러오기 가능
        // 만약 PlayerPrefs을 초기화하고 싶다면, `Edit` > `Clear All PalyerPrefs`
    }

    public void OpenRoomSettingMenu()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
            return;

        MenuManager.Instance.OpenMenu("room setting");
    }
}
