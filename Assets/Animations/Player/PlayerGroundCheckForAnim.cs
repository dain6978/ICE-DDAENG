using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheckForAnim : MonoBehaviour
{
    PlayerMoveForAnim playerMoveForAnim;

    private void Awake()
    {
        playerMoveForAnim = GetComponentInParent<PlayerMoveForAnim>(); //�θ��� ������Ʈ get
    }

    //GroundCheck�� �� (��, �÷��̾ �ƴ� �ٸ� Ʈ����)�� ���� ���ĳ� ��� �ִ� ��� SetGroundedState�� true��, ���� �ʰ� �ִ� ��� false��
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

    //�� Ʈ���Ŷ� �ݸ����� �� �� ����????
}
