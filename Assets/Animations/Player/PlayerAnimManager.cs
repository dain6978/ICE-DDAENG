using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    /*[HideInInspector]*/ public float moveMagnitude = 0;

    //GameObject playerAnimal;
    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    { 
        playerAnimator = GetComponentInChildren<Animator>(); //�׽�Ʈ��
        //playerAnimator = transform.GetChild(5).GetComponent<Animator>();
        // MeshRenderer�� �ڽ�(Bunny)�� �ڽ�(Mesh)�� �ڽ�(Bunny/Face) ������Ʈ
    }

    // Update is called once per frame
    private void Update()
    {
        playerAnimator.SetFloat("Move", moveMagnitude);

        if (Input.GetKeyDown(KeyCode.K)) //�׽�Ʈ��
        {
            playerAnimator.SetTrigger("Damage");
            playerAnimator.SetBool("Die", true);
        }
    }

    public void JumpAnim()
    {
        playerAnimator.SetTrigger("Jump");
    }


}
