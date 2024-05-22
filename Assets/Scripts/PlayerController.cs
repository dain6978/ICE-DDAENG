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

    [Header("Camera")]
    public GameObject cameraObject;
    Camera cam;
    [SerializeField] GameObject canvasForGun;
    public GameObject dieEffect;
    public int cameraZoom;

    [Header("Items")]
    [SerializeField] GameObject itemHolder;
    [SerializeField] Item[] items;
    [SerializeField] GameObject[] gunMeshes;

    public bool isEnd = false;
    int itemIndex;
    int previousItemIndex = -1;


    [SerializeField]
    int skinIndex;
    public int SkinIndex
    {
        get => skinIndex;
        set
        {
            skinIndex = value;
        }
    }


    [Header ("Player Movement")]
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    float moveSpeed;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    
    SkinnedMeshRenderer smr;
    PlayerAnimManager playerAnimManager;


    [Header("UI")]
    //[SerializeField] Image healthbarImage;
    private PlayerUI playerUI;
    private PlayerManager playerManager;
    public Transform snowGunTransform;
    public Transform damageGunTransform;
    int snowGunZoom = 1000;
    int damageGunZoom = 750;
    public GameObject targettingUI;


    //���⼭���� ice ����!!
    [Header("Ice")]
    public bool isIced = false;
    public int ice = 0;
    int iceMAX = 4;
    [SerializeField] GameObject snowmanObject;
    [SerializeField] GameObject animalObject;
    [SerializeField] GameObject brokeSnowmanObject;
    FrostEffect frostEffect;


    //ice���� �ʱ�ȭ �ð�
    float iceCurTime = 0f;
    float iceCoolTime = 4f;

    //isIced���� ���ӽð�
    float iceTime = 4f;

    bool canDance = false;
    [HideInInspector] public bool canClick = true;
    bool isZoom = false;
    bool isDie = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        frostEffect = cameraObject.GetComponent<FrostEffect>();
        cam = cameraObject.GetComponent<Camera>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        // PV.InstantiationData[0]: PlayerManager�� createController�� �����ϰ� ������ viewID�� ���� ����??
        // PhotonView���� Ư�� viewID�� ���� ���� ������Ʈ���� PlayerManager ������Ʈ�� �����´�. 


        playerAnimManager = GetComponentInChildren<PlayerAnimManager>();
        playerUI = GetComponentInChildren<PlayerUI>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            ChangeLayerRecursively(transform, LayerMask.NameToLayer("LocalPlayer"));
            EquipItem(0); //���� ������ �� �÷��̾�, �⺻���� 0�� �ε����� ���� ����
            snowmanObject.SetActive(false);
            brokeSnowmanObject.SetActive(false);
            
            //SetSkin(PlayerPrefs.GetInt("userskin"));

            //���콺 Ŀ�� �Ⱥ��̰�
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

        }
        else 
        { 
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //�ڱ� �ڽ��� �÷��̾�(���� �÷��̾�) ��Ʈ�ѷ��� �ƴ� ��� ī�޶� �ı� to �ڱ� �ڽ��� ī�޶� ����� �� �ֵ���
            //ī�޶�Ӹ� �ƴ϶� gameObject �ı� -> ����� �����ʵ� �Բ� �ı� (cameraHolder�� ����� ������µ� �� �𸣰ڴ�...)
            Destroy(rb); //���� �÷��̾��� rigidbody�� ����ϵ��� �ı�
            Destroy(playerUI.playerCanvas);
        }
    }
    private void Update()
    {
        if (!PV.IsMine)
            return; //�÷��̾� ��Ʈ�ѷ��� �ڱ� �ڽ��� �÷��̾ ��Ʈ���� �� �ְ� 

        // ���� ������ �� �û�뿡�� ���
        if (canDance && playerAnimManager.playerAnimator.runtimeAnimatorController == playerAnimManager.dancingAnimator)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                playerAnimManager.DancingAnim1();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                playerAnimManager.DancingAnim2();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                playerAnimManager.DancingAnim3();
        }

        if (isEnd)
            return;

        Look();
        Zoom();
        Move();
        Jump();

        Item();
        Fire();
        CheckIce();

        if (isDie)
            return;

        // �÷��̾� ���� �߶� ����
        if (transform.position.y < -8f)
            Die();

        if (iceCurTime < iceCoolTime)
            iceCurTime += Time.deltaTime;
        else
            if (ice != 0)
            {
                ice = 0;
                playerUI.ResetIce();
                frostEffect.FrostAmount = 0;
            }
        
    }



    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("isEnd"))
        {
            isEnd = (bool)propertiesThatChanged["isEnd"];
        }
    }

    private void ChangeLayerRecursively(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;

        foreach (Transform child in trans.transform)
        {
            ChangeLayerRecursively(child, layer);
        }
    }

    void Zoom()
    {
        if (isIced)
            return;

        if (!canClick)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            cameraZoom *= -1;
            snowGunZoom *= -1;
            damageGunZoom *= -1;
            cam.fieldOfView += cameraZoom;
            //snowGunTransform.localPosition = new Vector3(snowGunTransform.localPosition.x+snowGunZoom, snowGunTransform.localPosition.y, snowGunTransform.localPosition.z);
            //damageGunTransform.localPosition = new Vector3(damageGunTransform.localPosition.x + damageGunZoom, damageGunTransform.localPosition.y, damageGunTransform.localPosition.z);
            targettingUI.SetActive(!targettingUI.activeSelf);
        }
       
    }


    //ice ���� üũ
    void CheckIce()
    {
        if (ice >= iceMAX && !isIced)
        {
            isIced = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            AudioManager.Instacne.PlaySFX("Freeze");
            PV.RPC("RPC_Ice", RpcTarget.All, true);

            Invoke("ResetIce", iceTime);
        }
    }

    //ice �ʱ�ȭ
    void ResetIce()
    {
        if (isDie)
        {
            Debug.Log("�׾���!!");

            return;
        }
        Debug.Log("�׾��ٴϱ� ���ϳ�!!");

        ice = 0;
        isIced = false;
        playerUI.ResetIce();
        frostEffect.ResetFrost();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        PV.RPC("RPC_Ice", RpcTarget.All, false);
    }

    [PunRPC]
    void RPC_Ice(bool Ice)
    {
        animalObject.SetActive(!Ice);
        snowmanObject.SetActive(Ice);
    }

    

    void Item()
    {
        if (isIced)
            return;

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString())) //GetKeyDonw("string")�̴ϱ� ToString() �� ��
            {
                PV.RPC(nameof(EquipItem), RpcTarget.All, i);
                break;
            }
        }

        int mouseWheel = (int)Input.GetAxisRaw("Mouse ScrollWheel");
        if (mouseWheel == 0) return;

        itemIndex = (itemIndex + mouseWheel) % (items.Length);
        if (itemIndex < 0) itemIndex *= -1;

        
        PV.RPC(nameof(EquipItem), RpcTarget.All, itemIndex);
        
    }

    void Fire()
    {
        if (!canClick)
            return;

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

        playerAnimManager.MoveAnim(moveDir);
    }


    void Jump()
    {
        if (isIced || !grounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space)) //�����̽� Ű ������ �� ���� ��������� ���� (�ߺ� ���� ����)
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
        if (!PV.IsMine || isEnd)
            return;

        //Update �Լ����� ����� moveAmount�� �������� FixedUpdate���� ���� PlayerController�� rigidbody�� ����
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }


//���� 

    //showing and hiding items
    [PunRPC]
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return; //�Ȱ��� Ű �� �� ������ �ƹ� ���� X

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);
        gunMeshes[itemIndex].SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false); //item ��ü�ϸ� �� ���� item ��Ȱ��ȭ
            gunMeshes[previousItemIndex].SetActive(false);
        }

        if (PV.IsMine)
        {
            AudioManager.Instacne.PlaySFX(AudioManager.Instacne.sfx[itemIndex].name);
        }

        previousItemIndex = itemIndex;
    }


    public void SetCharacterSkin()
    {
        if (PV.IsMine)
            PV.RPC(nameof(SetSkin), RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["skinIndex"]);
    }

    [PunRPC]
    void SetSkin(int _index)
    {
        //Debug.Log($"name: {PV.Owner.NickName}, userskin: { _index} ");
        smr.sharedMesh = PlayerSkinManager.Instance.Meshs[_index];
        smr.material = PlayerSkinManager.Instance.Materials[_index];
    }

    //IDamageable �������̽��� TakeDamage �Լ� ����
    public void TakeDamage() //runs on the shooter's computer
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner);
        
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
    void RPC_TakeDamage(PhotonMessageInfo info) // runs on everyone's computer, but the '!PV.IsMine' check makes it only run on the victim's computer
    {

        if (isIced && !isDie)
        {
            dieEffect.SetActive(true);

            playerUI.HideAim();
            playerUI.HideIce();
            playerUI.ShowRespawn();
            PV.RPC("RPC_Break", RpcTarget.All);
            PlayerManager.Find(info.Sender).GetKill();
            AudioManager.Instacne.PlaySFX("Destruction_die");
            Die();
        }
    }

    [PunRPC]
    void RPC_Break()
    {
        snowmanObject.SetActive(false);
        brokeSnowmanObject.SetActive(true);
    }


    [PunRPC]
    void RPC_TakeSnow() 
    {

        playerAnimManager.DamageAnim();
        AudioManager.Instacne.PlaySFX("Hit");
        iceCurTime = 0;
        ice++;
        frostEffect.AddFrost();
        playerUI.AddIce();

    }


    public void SetRanking(int ranking)
    {
        isEnd = true;
        if (ranking < 3)
        {
            PV.RPC(nameof(RPC_SetRanking), RpcTarget.All, ranking);
            canDance = true;
        }

        //Debug.Log($"{ranking}��, {photonView.name}, isEnd: {isEnd}");
        //�Է¹������ֵ��� �����ؾ� ��.

        this.gameObject.GetComponentInChildren<PlayerUI>().Hide();
        cameraObject.SetActive(false);
        canvasForGun.SetActive(false);
        targettingUI.SetActive(false);
        frostEffect.ResetFrost();

       RankingManager.Instance.RankingCameraOn();
    }

    [PunRPC]
    void RPC_SetRanking(int ranking)
    {
        brokeSnowmanObject.SetActive(false);
        snowmanObject.SetActive(false);
        animalObject.SetActive(true);

        if (playerAnimManager != null)
            playerAnimManager.SetDancingMode();

        itemHolder.SetActive(false);

        this.transform.position = RankingManager.Instance.rankingPoints[ranking].position;
        this.transform.rotation = RankingManager.Instance.rankingPoints[ranking].rotation;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        //rb.mass = 10;
    }

    void Die()
    {
        if (isDie) return;
        isDie = true;
        playerManager.Die();
        PV.ObservedComponents.Clear();
    }
    //public void SetPlayersEnded()
    //{
    //    PlayerUI playerUI = this.gameObject.GetComponentInChildren<PlayerUI>();
    //    if (playerUI != null)
    //        this.gameObject.GetComponentInChildren<PlayerUI>().Hide();
    //    //this.gameObject.GetComponentInChildren<PlayerAnimManager>().SetDancingMode();


    //    PV.RPC("RPC_SetPlayersEnded", RpcTarget.All);
    //}

    //[PunRPC]
    //void RPC_SetPlayersEnded()
    //{
    //    brokeSnowmanObject.SetActive(false);
    //    snowmanObject.SetActive(false);
    //    animalObject.SetActive(true);

    //    if (playerAnimManager != null)
    //    {
    //        playerAnimManager.SetDancingMode();
    //    }
    //    itemHolder.SetActive(false);
    //}
}