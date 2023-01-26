using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable //IDamageable �������̽� ���� -> �������̽��� �Լ� �ݵ�� �����ؾ� ��
{
    PhotonView PV;

    [Header ("Items")]
    [SerializeField] Item[] items;

    int itemIndex;
    int previousItemIndex = -1;


    [Header ("Player Movement")]
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;


    [Header("Health")]
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject playerUI;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        // PV.InstantiationData[0]: PlayerManager�� createController�� �����ϰ� ������ viewID�� ���� ����??
        // PhotonView���� Ư�� viewID�� ���� ���� ������Ʈ���� PlayerManager ������Ʈ�� �����´�. 
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0); //���� ������ �� �÷��̾�, �⺻���� 0�� �ε����� ���� ����
        }
        else 
        { 
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //�ڱ� �ڽ��� �÷��̾�(���� �÷��̾�) ��Ʈ�ѷ��� �ƴ� ��� ī�޶� �ı� to �ڱ� �ڽ��� ī�޶� ����� �� �ֵ���
            //ī�޶�Ӹ� �ƴ϶� gameObject �ı� -> ����� �����ʵ� �Բ� �ı� (cameraHolder�� ����� ������µ� �� �𸣰ڴ�...)
            Destroy(rb);
            //���� �÷��̾��� rigidbody�� ����ϵ��� �ı�
            Destroy(playerUI); //���� �÷��̾��� UI (health��)�� ���
        }
    }
    private void Update()
    {
        if (!PV.IsMine)
            return; //�÷��̾� ��Ʈ�ѷ��� �ڱ� �ڽ��� �÷��̾ ��Ʈ���� �� �ְ� 
        
        Look();
        Move();
        Jump();

        Item();

        // �÷��̾� ���� �߶� ����
        if (transform.position.y < -10f)
        {
            Die();
        }
    }

    void Item()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString())) //GetKeyDonw("string")�̴ϱ� ToString() �� ��
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) //scrolling Up 
        {
            if (itemIndex >= items.Length - 1) //������ ���� �ʰ� x ���� ���� 
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1); //start �Լ����� EquipItem(0) -> itemIndex ����Ʈ��: 0
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) //scrolling Donw
        {
            if (itemIndex <= 0) //������ ���� �ʰ� x ���� ���� 
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

        if (Input.GetMouseButtonDown(0)) //���콺 ���� ��ư ������ �ش� �ѿ� ���� Use (shoot)
        {
            items[itemIndex].Use();
        }
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
    
    void Look()
    {
        //mouse x�� ����(2�������� x������ ���콺 �����ӿ� ����), 3�������� y���� �߽����� playerController�� ȸ��
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f); //ī�޶� ȸ�� ���� (�ּ�: -90, �ִ�: +90)

        //mouse y�� ����(2�������� y������ ���콺 �����ӿ� ����), 3�������� x���� �߽����� camerHolder�� ȸ��
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    //������Ʈ �Լ��� �� �����Ӹ��� ȣ�������, fixedUpdate �Լ��� ������ ���ݸ��� ȣ��ȴ� 
    //���� ��� ���� �� ������ ����� �� ��ǻ���� fps�� ������ �� �޵��� fixedUpdate�� ���´�
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        //Update �Լ����� ����� moveAmount�� �������� FixedUpdate���� ���� PlayerController�� rigidbody�� ����
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }


//���� 

    //showing and hiding items
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return; //�Ȱ��� Ű �� �� ������ �ƹ� ���� X

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false); //item ��ü�ϸ� �� ���� item ��Ȱ��ȭ
        }

        previousItemIndex = itemIndex;

        
        // ��Ʈ��ũ ���� �÷��̾�� ���� ���� ��ü(EquipItem)�� ���� Syncing
        
        //Send out syncing datas (customPlayerProperties) from local player to network
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable(); //��Ʈ��ũ�� customPlayerProperties �����͸� �����ϱ� ����, �ؽ����̺� ����? (�ؽ����̺�: ��ųʸ� ���)
            hash.Add("ItemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    // ��ü ���� ���� � �÷��̾��� � �Ӽ��� ������Ʈ �� ������ ����Ǵ� �Լ�
    // called when information is received
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {//�ٸ� �÷��̾��� �Ӽ��� ������Ʈ ���� �� (�ٸ� �÷��̾ ���� ��ü���� ��) 
            EquipItem((int)changedProps["ItemIndex"]);
            //�ؽ� ���̺��� ItemIndex�� int�� ����ȯ�ϰ� EquipItem�� ���� ������ ��Ʈ��ũ�� pass...??
        }
    }

    //IDamageable �������̽��� TakeDamage �Լ� ����
    public void TakeDamage(float damage) //runs on the shooter's computer
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        //RPC ȣ���ϴ� ��: PV.RPC("�Լ� �̸�), Ÿ��, �Լ� �Ķ����) 
        //RpcTarget.All: ������ �ִ� ��� �÷��̾�� ���� ����
    }

    //to sync damage, RPC ���
    [PunRPC]
    void RPC_TakeDamage(float damage) // runs on everyone's computer, but the '!PV.IsMine' check makes it only run on the victim's computer
    {
        //���� ���� ������ ����, �������� ���� victim �÷��̾��� ��ǻ�Ϳ����� Debug.Log �ڵ� ����, �������� return
        if (!PV.IsMine)
            return;

        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // �÷��̾� �Ŵ������� �÷��̾��� death�� respawning ����
        playerManager.Die();
    }
}