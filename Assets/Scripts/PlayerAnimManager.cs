using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimManager : MonoBehaviour, IPunObservable
{
    public RuntimeAnimatorController gameAnimator;
    public RuntimeAnimatorController dancingAnimator;

    [HideInInspector]
    public float moveMagnitude = 0f;
    public float playerSpineRotation = 0f;

    //GameObject playerAnimal;
    [HideInInspector]
    public Animator playerAnimator;
    
    Transform playerSpine;



    private void Awake()
    {
        playerAnimator = GetComponentInParent<Animator>();
        playerSpine = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);

        playerAnimator.SetFloat("BlendDamage", 0.7f);
        playerAnimator.SetFloat("BlendShoot", 0.6f);

        //animPV = this.gameObject.GetComponentInParent<PhotonAnimatorView>();
    }



    private void LateUpdate()
    {
        //playerSpineRotation == ī�޶��� ȸ���� (verticalLookRotation) 
        //mouse y�� ����(2�������� y������ ���콺 �����ӿ� ����), 3�������� z���� �߽����� Spine�� ȸ��
        playerSpine.localRotation = Quaternion.Euler(0, 0, playerSpineRotation * 0.8f);
    }


    public void DancingAnim1()
    {
        playerAnimator.SetTrigger("Dance1");
    }
    public void DancingAnim2()
    {
        playerAnimator.SetTrigger("Dance2");
    }

    public void DancingAnim3()
    {
        playerAnimator.SetTrigger("Dance3");
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
        }
        else
        {
            playerSpineRotation = (float)stream.ReceiveNext();
        }
    }

    public void SetDancingMode()
    {
        Destroy(this.gameObject.GetComponent<PhotonAnimatorView>());
        playerAnimator.runtimeAnimatorController = dancingAnimator;
        playerSpineRotation = 0;

        PhotonAnimatorView animPV = this.gameObject.GetComponentInParent<PhotonAnimatorView>();
        animPV.SetParameterSynchronized("Dance1", PhotonAnimatorView.ParameterType.Trigger, PhotonAnimatorView.SynchronizeType.Discrete);
        animPV.SetParameterSynchronized("Dance2", PhotonAnimatorView.ParameterType.Trigger, PhotonAnimatorView.SynchronizeType.Discrete);
        animPV.SetParameterSynchronized("Dance3", PhotonAnimatorView.ParameterType.Trigger, PhotonAnimatorView.SynchronizeType.Discrete);
    }

}
