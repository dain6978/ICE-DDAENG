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
            gameObject.SetActive(false); //자기 자신의 username 볼 필요 없으니까 (destroy와 setActive(false) 구분! 아예 파괴하는 거랑 안 보이게 하는 것)
        }

        usernameText.text = playerPV.Owner.NickName;  //photon view를 소유하고 있는 어떤 플레이어의 닉네임
    }
}
