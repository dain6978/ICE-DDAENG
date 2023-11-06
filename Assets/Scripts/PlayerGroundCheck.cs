using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>(); //부모의 컴포넌트 get
    }

    //GroundCheck가 땅 (즉, 플레이어가 아닌 다른 트리거)에 닿은 직후나 닿고 있는 경우 SetGroundedState를 true로, 닿지 않고 있는 경우 false로
    private void OnTriggerEnter(Collider other)
    {
        playerController.SetGroundedState(true);
    }
    private void OnTriggerExit(Collider other)
    {
        playerController.SetGroundedState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        playerController.SetGroundedState(true);
    }

}
