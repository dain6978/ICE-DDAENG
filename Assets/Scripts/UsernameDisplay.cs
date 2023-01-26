using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] TMP_Text usernameText;

    private void Start()
    {
        if (playerPV.IsMine)
        {
            gameObject.SetActive(false); //�ڱ� �ڽ��� username �� �ʿ� �����ϱ� (destroy�� setActive(false) ����! �ƿ� �ı��ϴ� �Ŷ� �� ���̰� �ϴ� ��)
        }

        usernameText.text = playerPV.Owner.NickName;  //photon view�� �����ϰ� �ִ� � �÷��̾��� �г���
    }
}
