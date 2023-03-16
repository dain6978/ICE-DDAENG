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
}
