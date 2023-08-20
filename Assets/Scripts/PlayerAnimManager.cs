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
        //if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        //{
        //    moveMagnitude = 1.0f;
        //}
        //else
        //{
        //    moveMagnitude = 0.0f;
        //}
        //playerAnimator.SetFloat("Speed", moveMagnitude);
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

    public void MoveAnim(Vector3 moveDir)
    {
        playerAnimator.SetFloat("Speed", Vector3.SqrMagnitude(moveDir));
        
        if (Input.GetKey(KeyCode.LeftShift))
            playerAnimator.SetBool("Run", true);
        else
            playerAnimator.SetBool("Run", false);
    }



    // IPunObservable ��� �� �� �����ؾ� �ϴ� ������, �����͸� ��Ʈ��ũ ����� ���� ������ �ް� �ϰ� �ϴ� �ݹ� �Լ�
    // ������ ���� �߻��ϴ� ����� ����ȭ: OnPhotonSerializeView (����� stream�� �� �� �̻��� Ŭ���̾�Ʈ�� ������ ��쿡�� �аų� ����.)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //stream - �����͸� �ְ� �޴� ���
        if (stream.IsWriting) // ���� �����͸� ������ ���̶��
        {
            stream.SendNext(playerSpineRotation);
            //stream.SendNext(moveMagnitude);
        }
        else
        {
            playerSpineRotation = (float)stream.ReceiveNext();
            //moveMagnitude = (float)stream.ReceiveNext();
        }
    }
}
