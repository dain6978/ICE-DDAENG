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
            //HasKey: �ش� Ű�� ���� ���� bool ������ ��ȯ
        {
            usernameInput.text = PlayerPrefs.GetString("username"); 
            //username Ű�� �����ϸ�, �� �̹� ��ǲ�ʵ忡�� ���� �̸��� ������ �����Ͱ� �ִٸ�, �ش� �����͸� ���� ��Ʈ��ũ�� �г�������
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        { // ���� ������ ������ ������ ���� �̸�����
            usernameInput.text = "Player" + Random.Range(0, 1000).ToString("0000");
            OnUsernameInputValueChanged();
        }
    }
    
    //��ǲ �ʵ��� �ؽ�Ʈ�� ��ȭ�� ������ ȣ��Ǵ� �Լ�
    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("username", usernameInput.text); // SetString: ������ Ű�� string Ÿ���� �� ����
        //PlayerPrefs: ����Ƽ���� �����ϴ� ������ ���� Ŭ������, �������� ���� �� �ҷ����� ����
        // ���� PlayerPrefs�� �ʱ�ȭ�ϰ� �ʹٸ�, `Edit` > `Clear All PalyerPrefs`
    }

    public void OpenRoomSettingMenu()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
            return;

        MenuManager.Instance.OpenMenu("room setting");
    }
}
