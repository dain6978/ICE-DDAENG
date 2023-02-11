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
        playerAnimator = GetComponentInChildren<Animator>(); //테스트용
        //playerAnimator = transform.GetChild(5).GetComponent<Animator>();
        // MeshRenderer는 자식(Bunny)의 자식(Mesh)의 자식(Bunny/Face) 컴포넌트
        playerSpine = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
    }

    // Update is called once per frame
    private void Update()
    {
        playerAnimator.SetFloat("Speed", moveMagnitude);

        if (Input.GetKeyDown(KeyCode.K)) //테스트용
        {
            playerAnimator.SetTrigger("Damage");
            playerAnimator.SetBool("Die", true);
        }
    }

    private void LateUpdate()
    {
        //playerSpineRotation == 카메라의 회전값 (verticalLookRotation) 
        //mouse y에 따라(2차원에서 y축으로 마우스 움직임에 따라), 3차원에서 z축을 중심으로 Spine이 회전
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
