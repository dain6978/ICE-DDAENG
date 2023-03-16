using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimManager : MonoBehaviour, IPunObservable
{
    [HideInInspector]
    public float moveMagnitude = 0f;
    public float playerSpineRotation = 0f;

    //GameObject playerAnimal;
    Animator playerAnimator;
    Transform playerSpine;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInParent<Animator>();
        playerSpine = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);

        playerAnimator.SetFloat("BlendDamage", 0.7f);
        playerAnimator.SetFloat("BlendShoot", 0.6f);
    }

    // Update is called once per frame
    private void Update()
    {
        playerAnimator.SetFloat("Speed", moveMagnitude);
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

    // IPunObservable 상속 시 꼭 구현해야 하는 것으로, 데이터를 네트워크 사용자 간에 보내고 받고 하게 하는 콜백 함수
    // 갱신이 자주 발생하는 경우의 동기화: OnPhotonSerializeView (참고로 stream은 두 명 이상의 클라이언트가 접속한 경우에만 읽거나 쓴다.)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //stream - 데이터를 주고 받는 통로
        if (stream.IsWriting) // 내가 데이터를 보내는 중이라면
        {
            stream.SendNext(playerSpineRotation);
        }
        else
        {
            playerSpineRotation = (float)stream.ReceiveNext();
        }
    }
}
