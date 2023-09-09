using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable //IDamageable 인터페이스 부착 -> 인터페이스의 함수 반드시 구현해야 함
{
    PhotonView PV;

    [Header ("Items")]
    [SerializeField] Item[] items;
    [SerializeField] GameObject[] gunMeshes;

    bool isEnd = false;
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
    

    GameObject playerAnimal;
    //SkinnedMeshRenderer[] playerMeshs; // 동물 메시 & face 메시
    SkinnedMeshRenderer smr;
    PlayerAnimManager playerAnimManager;


    [Header("Health")]
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject playerUI;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;
    UIManager uiManager;



    //여기서부터 ice 관련!!
    [Header("Ice")]
    public bool isIced = false;
    public int ice = 0;
    int iceMAX = 4;
    public GameObject snowmanObject;
    public GameObject playerObject;

    //ice개수 초기화 시간
    float iceCurTime = 0f;
    float iceCoolTime = 5f;

    //isIced상태 지속시간
    float iceTime = 5f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        smr = GetComponentInChildren<SkinnedMeshRenderer>();

        
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        // PV.InstantiationData[0]: PlayerManager의 createController에 생성하고 전송한 viewID에 대한 정보??
        // PhotonView에서 특정 viewID를 가진 게임 오브젝트에서 PlayerManager 컴포넌트를 가져온다. 

        //playerAnimal = transform.GetChild(5).gameObject;
        //playerMeshs = playerAnimal.GetComponentsInChildren<SkinnedMeshRenderer>();
       
        //PhotonNetwork.LocalPlayer.TagObject = gameObject;

        playerAnimManager = GetComponentInChildren<PlayerAnimManager>();
    }

    private void Start()
    {
       
        if (PV.IsMine)
        {
            EquipItem(0); //게임 시작할 때 플레이어, 기본으로 0번 인덱스의 무기 장착
            snowmanObject.SetActive(false);

            
            //SetSkin(PlayerPrefs.GetInt("userskin"));

            //마우스 커서 안보이게
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            // 플레이어 1인칭 시점에서 자기 자신의 모습 (mesh) 안 보이게 설정
            //Destroy(playerMeshs[0]);
            //Destroy(playerMeshs[1]);

        }
        else 
        { 
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //자기 자신의 플레이어(로컬 플레이어) 컨트롤러가 아닐 경우 카메라 파괴 to 자기 자신의 카메라만 사용할 수 있도록
            //카메라뿐만 아니라 gameObject 파괴 -> 오디오 리스너도 함께 파괴 (cameraHolder를 사용한 이유라는데 잘 모르겠당...)
            Destroy(rb); 
            //로컬 플레이어의 rigidbody만 사용하도록 파괴
            Destroy(playerUI); //로컬 플레이어의 UI (health바)만 사용
        }
    }
    private void Update()
    {
        if (!PV.IsMine || isEnd)
            return; //플레이어 컨트롤러가 자기 자신의 플레이어만 컨트롤할 수 있게 

        
        Look();
        Move();
        Jump();

        Item();
        Fire();
        CheckIce();

        // 플레이어 무한 추락 방지
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



    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("isEnd"))
        {
            isEnd = (bool)propertiesThatChanged["isEnd"];
        }
    }

    //ice 개수 체크
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

    //ice 초기화
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
            if (Input.GetKeyDown((i + 1).ToString())) //GetKeyDonw("string")이니까 ToString() 한 것
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
        if (Input.GetButton("Fire1")) //마우스 왼쪽 버튼 누르면 해당 총에 대해 Use (shoot)
        {
            if (isIced)
                return;

            items[itemIndex].Use();
            playerAnimManager.ShootAnim(); // 플레이어의 Shoot 애니메이션 (일단 총이랑 관련 X)
        }
    }

    void Move()
    {
        if (isIced)
            return;

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

        playerAnimManager.MoveAnim(moveDir);
    }


    void Jump()
    {
        if (isIced)
            return;

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
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -40f, 80f); //카메라 회전 각도 (최소: -40, 최대: +80)

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        //mouse y에 따라(2차원에서 y축으로 마우스 움직임에 따라), 3차원에서 x축을 중심으로 camerHolder가 회전
        playerAnimManager.playerSpineRotation = verticalLookRotation;
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
        if (!PV.IsMine || isEnd)
            return;

        //Update 함수에서 계산한 moveAmount를 바탕으로 FixedUpdate마다 실제 PlayerController의 rigidbody에 영향
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }


//무기 

    //showing and hiding items
    [PunRPC]
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return; //똑같은 키 두 번 누르면 아무 반응 X

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);
        gunMeshes[itemIndex].SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false); //item 교체하면 그 전의 item 비활성화
            gunMeshes[previousItemIndex].SetActive(false);
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
        Debug.Log($"name: {PV.Owner.NickName}, userskin: { _index} ");
        smr.sharedMesh = PlayerSkinManager.Instance.Meshs[_index];
        smr.material = PlayerSkinManager.Instance.Materials[_index];
    }

    // 전체 게임 동안 어떤 플레이어의 어떤 속성이 업데이트 될 때마다 실행되는 함수
    // called when information is received
    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //{
        

    //    if (changedProps.ContainsKey("skinIndex") && !PV.IsMine && targetPlayer == PV.Owner)
    //    {
    //        SetSkin((int)changedProps["skinIndex"]);

    //    }

    //    //if (changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner)
    //    //{//다른 플레이어의 itemindex 속성이 업데이트 됐을 때 (다른 플레이어가 무기 교체했을 때) 
    //    //    EquipItem((int)changedProps["ItemIndex"]);
    //    //    //해시 테이블의 ItemIndex를 int로 형변환하고 EquipItem에 대한 정보를 네트워크로 pass...??
    //    //}
    //}

    //IDamageable 인터페이스의 TakeDamage 함수 구현
    public void TakeDamage(float damage) //runs on the shooter's computer
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
        //RPC 호출하는 법: PV.RPC("함수 이름), 타겟, 함수 파라미터) 
        //RpcTarget.All: 서버에 있는 모든 플레이어에게 정보 전달
        //PV.Owner은 데미지 입는 본인한테만.
    }

    public void TakeSnow() 
    {
        PV.RPC(nameof(RPC_TakeSnow), PV.Owner);
        //RPC 호출하는 법: PV.RPC("함수 이름), 타겟, 함수 파라미터) 
        //RpcTarget.All: 서버에 있는 모든 플레이어에게 정보 전달
    }

    //to sync damage, RPC 사용
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
            
            //Sender의 info전송하고 info.Sender의 GetKill()호출
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
        // 플레이어 매니저에서 플레이어의 death와 respawning 관리
        playerManager.Die();
    }
}