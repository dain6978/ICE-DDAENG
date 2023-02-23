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
    float moveSpeed;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    GameObject playerAnimal;
    SkinnedMeshRenderer[] playerMeshs; // ���� �޽� & face �޽�

    PlayerAnimManager playerAnimManager;


    [Header("Health")]
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject playerUI;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;
    

    //���⼭���� ice ����!!
    [Header("Ice")]
    public bool isIced = false;
    public int ice = 0;
    int iceMAX = 4;
    public GameObject snowmanObject;
    public GameObject playerObject;

    //ice���� �ʱ�ȭ �ð�
    float iceCurTime = 0f;
    float iceCoolTime = 5f;

    //isIced���� ���ӽð�
    float iceTime = 5f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        // PV.InstantiationData[0]: PlayerManager�� createController�� �����ϰ� ������ viewID�� ���� ����??
        // PhotonView���� Ư�� viewID�� ���� ���� ������Ʈ���� PlayerManager ������Ʈ�� �����´�. 

        //playerAnimal = transform.GetChild(5).gameObject;
        //playerMeshs = playerAnimal.GetComponentsInChildren<SkinnedMeshRenderer>();

        playerAnimManager = GetComponent<PlayerAnimManager>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0); //���� ������ �� �÷��̾�, �⺻���� 0�� �ε����� ���� ����
            snowmanObject.SetActive(false);
            

            //���콺 Ŀ�� �Ⱥ��̰�
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            // �÷��̾� 1��Ī �������� �ڱ� �ڽ��� ��� (mesh) �� ���̰� ����
            //Destroy(playerMeshs[0]);
            //Destroy(playerMeshs[1]);

        }
        else 
        { 
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //�ڱ� �ڽ��� �÷��̾�(���� �÷��̾�) ��Ʈ�ѷ��� �ƴ� ��� ī�޶� �ı� to �ڱ� �ڽ��� ī�޶� ����� �� �ֵ���
            //ī�޶�Ӹ� �ƴ϶� gameObject �ı� -> ����� �����ʵ� �Բ� �ı� (cameraHolder�� ����� ������µ� �� �𸣰ڴ�...)
            Destroy(rb);
            //���� �÷��̾��� rigidbody�� ����ϵ��� �ı�
            Destroy(playerUI); //���� �÷��̾��� UI (health��)�� ���
            Destroy(playerAnimManager); //�ڱ� �ڽ��� ���� �ƴ� �ִϸ����� �ı�... ���̰� ����̴µ� �ǹ̰� �ֳ�?
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
        CheckIce();

        // �÷��̾� ���� �߶� ����
        if (transform.position.y < -10f)
        {
            Die();
        }

        if (iceCurTime < iceCoolTime)
        {
            iceCurTime += Time.deltaTime;
        }
        else
            ice = 0;
            
    }

    //ice ���� üũ
    void CheckIce()
    {
        if (ice >= iceMAX && !isIced)
        {
            isIced = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            PV.RPC("RPC_Ice", RpcTarget.All, true);

            Invoke("ResetIce", iceTime);
        }
            
    }

    //ice �ʱ�ȭ
    void ResetIce()
    {
        ice = 0;
        isIced = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        PV.RPC("RPC_Ice", RpcTarget.All, false);
    }

    [PunRPC]
    void RPC_Ice(bool Ice)
    {
        playerObject.SetActive(!Ice);
        snowmanObject.SetActive(Ice);
        
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

        if (Input.GetButton("Fire1")) //���콺 ���� ��ư ������ �ش� �ѿ� ���� Use (shoot)
        {
            if (isIced)
                return;

            items[itemIndex].Use();
            playerAnimManager.ShootAnim(); // �÷��̾��� Shoot �ִϸ��̼� (�ϴ� ���̶� ���� X)
        }
    }

    void Move()
    {
        if (isIced)
            return;

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //normalize�� �� ���� Ű�� ���ÿ� ������ ��(��: w & d) ������ move�ϴ� �� ����

        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * moveSpeed, ref smoothMoveVelocity, smoothTime);
        //moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed)
        // : left shift Ű�� ������ �ִ� ���¸� sprintSpeed��, �ƴϸ� walkSpeed�� �����̰� �ִ� ������ moveDir�� ����
        //smooth damp: �������� smooth�ϰ� ������ִ� ����

        playerAnimManager.moveMagnitude = Vector3.SqrMagnitude(moveDir);
        playerAnimManager.MoveAnim(moveSpeed);
    }

    void Jump()
    {
        if (isIced)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && grounded) //�����̽� Ű ������ �� ���� ��������� ���� (�ߺ� ���� ����)
        {
            rb.AddForce(transform.up * jumpForce);
            playerAnimManager.JumpAnim();
        }
    }
    
    void Look()
    {
        //mouse x�� ����(2�������� x������ ���콺 �����ӿ� ����), 3�������� y���� �߽����� playerController�� ȸ��
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -40f, 80f); //ī�޶� ȸ�� ���� (�ּ�: -40, �ִ�: +80)

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        //mouse y�� ����(2�������� y������ ���콺 �����ӿ� ����), 3�������� x���� �߽����� camerHolder�� ȸ��
        playerAnimManager.playerSpineRotation = verticalLookRotation;
        //playerSpine.transform.localRotation = Quaternion.Euler(0, 0, verticalLookRotation);
        //playerSpine.transform.position
        //mouse y�� ����(2�������� y������ ���콺 �����ӿ� ����), 3�������� z���� �߽����� Spine�� ȸ��
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
        if (changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner)
        {//�ٸ� �÷��̾��� itemindex �Ӽ��� ������Ʈ ���� �� (�ٸ� �÷��̾ ���� ��ü���� ��) 
            EquipItem((int)changedProps["ItemIndex"]);
            //�ؽ� ���̺��� ItemIndex�� int�� ����ȯ�ϰ� EquipItem�� ���� ������ ��Ʈ��ũ�� pass...??
        }
    }

    //IDamageable �������̽��� TakeDamage �Լ� ����
    public void TakeDamage(float damage) //runs on the shooter's computer
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
        //RPC ȣ���ϴ� ��: PV.RPC("�Լ� �̸�), Ÿ��, �Լ� �Ķ����) 
        //RpcTarget.All: ������ �ִ� ��� �÷��̾�� ���� ����
        //PV.Owner�� ������ �Դ� �������׸�.
    }

    public void TakeSnow() 
    {
        PV.RPC(nameof(RPC_TakeSnow), PV.Owner);
        //RPC ȣ���ϴ� ��: PV.RPC("�Լ� �̸�), Ÿ��, �Լ� �Ķ����) 
        //RpcTarget.All: ������ �ִ� ��� �÷��̾�� ���� ����
    }

    //to sync damage, RPC ���
    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info) // runs on everyone's computer, but the '!PV.IsMine' check makes it only run on the victim's computer
    {
        if (isIced)
        {
            currentHealth -= damage;
            healthbarImage.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
            
            //Sender�� info�����ϰ� info.Sender�� GetKill()ȣ��
            PlayerManager.Find(info.Sender).GetKill();
        }


    }

    [PunRPC]
    void RPC_TakeSnow() 
    {

        playerAnimManager.DamageAnim();

        iceCurTime = 0;
        ice++;

    }

    void Die()
    {
        // �÷��̾� �Ŵ������� �÷��̾��� death�� respawning ����
        playerManager.Die();
    }
}