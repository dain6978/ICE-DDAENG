using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    /*[HideInInspector]*/
    public float moveMagnitude = 0f;
    public float playerSpineRotation = 0f;

    //GameObject playerAnimal;
    Animator playerAnimator;
    Transform playerSpine;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>(); //�׽�Ʈ��
        //playerAnimator = transform.GetChild(5).GetComponent<Animator>();
        // MeshRenderer�� �ڽ�(Bunny)�� �ڽ�(Mesh)�� �ڽ�(Bunny/Face) ������Ʈ
        playerSpine = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
    }

    // Update is called once per frame
    private void Update()
    {
        playerAnimator.SetFloat("Speed", moveMagnitude);

        if (Input.GetKeyDown(KeyCode.K)) //�׽�Ʈ��
        {
            playerAnimator.SetTrigger("Damage");
            playerAnimator.SetBool("Die", true);
        }
    }

    private void LateUpdate()
    {
        //playerSpineRotation == ī�޶��� ȸ���� (verticalLookRotation) 
        //mouse y�� ����(2�������� y������ ���콺 �����ӿ� ����), 3�������� z���� �߽����� Spine�� ȸ��
        playerSpine.localRotation = Quaternion.Euler(0, 0, playerSpineRotation * 0.8f);
    }

    public void JumpAnim()
    {
        playerAnimator.SetTrigger("Jump");
    }

    public void DamageAnim()
    {
        playerAnimator.SetTrigger("Damage");
    }

    public void ShootAnim()
    {
        playerAnimator.SetTrigger("Shoot");
    }

    public void MoveAnim(float moveSpeed)
    {
        if (moveSpeed == 2.5f)
        {
            playerAnimator.SetBool("Run", false);
        }
        else if (moveSpeed == 5f)
        {
            playerAnimator.SetBool("Run", true);
        }
    }

}
