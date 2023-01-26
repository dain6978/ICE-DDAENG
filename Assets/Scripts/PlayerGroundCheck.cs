using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>(); //�θ��� ������Ʈ get
    }

    //GroundCheck�� �� (��, �÷��̾ �ƴ� �ٸ� Ʈ����)�� ���� ���ĳ� ��� �ִ� ��� SetGroundedState�� true��, ���� �ʰ� �ִ� ��� false��
    private void OnTriggerEnter(Collider other)
    {
        if (other == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == playerController.gameObject)
            return;
        
        playerController.SetGroundedState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    //�� Ʈ���Ŷ� �ݸ����� �� �� ����????
}
