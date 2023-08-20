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
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float moveSpeed;
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PlayerAnimManager playerAnimManager;

    GameObject playerAnimal;
    SkinnedMeshRenderer[] playerMeshs; // 동물 메시 & face 메시


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerAnimManager = GetComponent<PlayerAnimManager>();

        playerAnimal = transform.GetChild(0).gameObject;
        playerMeshs = playerAnimal.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        //Destroy(playerMeshs[0]);
        //Destroy(playerMeshs[1]);
    }

    private void Update()
    {
        Look();
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //normalize는 두 개의 키를 동시에 눌렀을 때(예: w & d) 빠르게 move하는 것 방지

        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * moveSpeed, ref smoothMoveVelocity, smoothTime);
        //moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed)
        // : left shift 키를 누르고 있는 상태면 sprintSpeed를, 아니면 walkSpeed를 움직이고 있는 방향인 moveDir에 곱함
        //smooth damp: 움직임을 smooth하게 만들어주는 역할

        playerAnimManager.moveMagnitude = Vector3.SqrMagnitude(moveDir);
        playerAnimManager.MoveAnim(moveDir);

            
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded) //스페이스 키 눌렀을 때 땅에 닿아있으면 점프 (중복 점프 방지)
        {
            rb.AddForce(transform.up * jumpForce);
            playerAnimManager.JumpAnim();
        }
    }

    void Look()
    {
        //mouse x에 따라(2차원에서 x축으로 마우스 움직임에 따라), 3차원에서 y축을 중심으로 playerController가 회전
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -40f, 80f); //카메라 회전 각도 (최소: -90, 최대: +90)

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        //mouse y에 따라(2차원에서 y축으로 마우스 움직임에 따라), 3차원에서 x축을 중심으로 camerHolder가 회전
        playerAnimManager.playerSpineRotation = verticalLookRotation;
        //playerSpine.transform.localRotation = Quaternion.Euler(0, 0, verticalLookRotation);
        //playerSpine.transform.position
        //mouse y에 따라(2차원에서 y축으로 마우스 움직임에 따라), 3차원에서 z축을 중심으로 Spine이 회전


    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    //업데이트 함수는 매 프레임마다 호출되지만, fixedUpdate 함수는 고정된 가격마다 호출된다 
    //따라서 모든 물리 및 움직임 계산은 각 컴퓨터의 fps에 영향을 덜 받도록 fixedUpdate에 적는다
    private void FixedUpdate()
    {
        //Update 함수에서 계산한 moveAmount를 바탕으로 FixedUpdate마다 실제 PlayerController의 rigidbody에 영향
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

}