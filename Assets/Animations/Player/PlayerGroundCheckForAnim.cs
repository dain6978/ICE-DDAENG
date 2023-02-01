using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheckForAnim : MonoBehaviour
{
    PlayerMoveForAnim playerMoveForAnim;

    private void Awake()
    {
        playerMoveForAnim = GetComponentInParent<PlayerMoveForAnim>(); //부모의 컴포넌트 get
    }

    //GroundCheck가 땅 (즉, 플레이어가 아닌 다른 트리거)에 닿은 직후나 닿고 있는 경우 SetGroundedState를 true로, 닿지 않고 있는 경우 false로
    private void OnTriggerEnter(Collider other)
    {
        if (other == playerMoveForAnim.gameObject)
            return;

        playerMoveForAnim.SetGroundedState(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == playerMoveForAnim.gameObject)
            return;

        playerMoveForAnim.SetGroundedState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == playerMoveForAnim.gameObject)
            return;

        playerMoveForAnim.SetGroundedState(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerMoveForAnim.gameObject)
            return;

        playerMoveForAnim.SetGroundedState(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerMoveForAnim.gameObject)
            return;

        playerMoveForAnim.SetGroundedState(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerMoveForAnim.gameObject)
            return;

        playerMoveForAnim.SetGroundedState(true);
    }

    //왜 트리거랑 콜리전을 둘 다 쓰지????
}
