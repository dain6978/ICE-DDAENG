using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    public float moveMagnitude = 0;

    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        playerAnimator.SetFloat("Move", moveMagnitude);

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


}
