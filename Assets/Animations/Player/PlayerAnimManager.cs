using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    /*[HideInInspector]*/
    public float moveMagnitude = 0;

    //GameObject playerAnimal;
    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>(); //테스트용
        //playerAnimator = transform.GetChild(5).GetComponent<Animator>();
        // MeshRenderer는 자식(Bunny)의 자식(Mesh)의 자식(Bunny/Face) 컴포넌트
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

    public void JumpAnim()
    {
        playerAnimator.SetTrigger("Jump");
    }

    public void DamageAnim()
    {
        playerAnimator.SetTrigger("Damage");
    }

    public void MoveAnim(float moveSpeed)
    {
        if (moveSpeed == 2.5f)
        {
            playerAnimator.SetBool("Run", false);
        }
        else if (moveSpeed == 6f)
        {
            playerAnimator.SetBool("Run", true);
        }
    }
}
