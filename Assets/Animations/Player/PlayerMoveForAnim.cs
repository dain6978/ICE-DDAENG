using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerMoveForAnim : MonoBehaviour
{

    //[Header("Player Movement")]
    //[SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Look();
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //normalize�� �� ���� Ű�� ���ÿ� ������ ��(��: w & d) ������ move�ϴ� �� ����

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        //moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed)
        // : left shift Ű�� ������ �ִ� ���¸� sprintSpeed��, �ƴϸ� walkSpeed�� �����̰� �ִ� ������ moveDir�� ����
        //smooth damp: �������� smooth�ϰ� ������ִ� ����
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded) //�����̽� Ű ������ �� ���� ��������� ���� (�ߺ� ���� ����)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    //void Look()
    //{
    //    //mouse x�� ����(2�������� x������ ���콺 �����ӿ� ����), 3�������� y���� �߽����� playerController�� ȸ��
    //    transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

    //    verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
    //    verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f); //ī�޶� ȸ�� ���� (�ּ�: -90, �ִ�: +90)

    //    //mouse y�� ����(2�������� y������ ���콺 �����ӿ� ����), 3�������� x���� �߽����� camerHolder�� ȸ��
    //    cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    //}

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    //������Ʈ �Լ��� �� �����Ӹ��� ȣ�������, fixedUpdate �Լ��� ������ ���ݸ��� ȣ��ȴ� 
    //���� ��� ���� �� ������ ����� �� ��ǻ���� fps�� ������ �� �޵��� fixedUpdate�� ���´�
    private void FixedUpdate()
    {
        //Update �Լ����� ����� moveAmount�� �������� FixedUpdate���� ���� PlayerController�� rigidbody�� ����
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

}