using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;
    PlayerAnimManager playerAnimManager;
    public Animator anim;

    bool canShoot = true;
    public bool canUse = false;
    int playerLayer = (1 << 8);


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerAnimManager = FindObjectOfType<PlayerAnimManager>();
    }
    
    public override void Use()
    {
        if (!canUse || !canShoot) return;
        Shoot();
    }

    private void Update()
    {
        if (Input.GetButtonUp("Fire1"))
            canShoot = true;
    }
    void Shoot()
    {
        canShoot = false;

        AudioManager.Instacne.PlaySFX("Fire_Damage");
        //raycast 
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //스크린(1인칭 카메라)의 중앙으로부터 뻗어 나오는 ray
        ray.origin = cam.transform.position; // ray의 시작점을 카메라의 위치로

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50.0f, (1 << 0) | (1 << 8) | (1 << 11)))
        {
            Debug.Log(hit.collider.gameObject);
            hit.collider.gameObject.GetComponentInParent<IDamageable>()?.TakeDamage();
            // '?' 표시: 게임 오브젝트가 IDamageable 인터페이스를 가지고 있을 때만 TakeDamage 함수가 호출된다는 의미
            // SingleShotGun 스크립트 (클래스)는 Gun과 Item 클래스를 상속받음 -> Item 클래스의 public 변수인 itemInfo에 접근 가능
            // (itemInfo를 인스턴스로 갖는 ItemInfo 클래스에는 아이템의 이름에 대한 정보를 담은 itemName 변수 있음)
            // damaga는 ItemInfo를 상속받은 GunInfo의 변수이기 때문에 GunInfo로 형변환 (언리얼 블루프린트 클래스 형변환 생각하면 될듯)

            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal); //모든 클라이언트에 대해 hit(raycast의 반환값)의 위치를 인자로 하는 RPC_Shoot 함수 호출
            anim.CrossFadeInFixedTime("Fire", 0.01f);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        //??????
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj;
            bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            //  hitPosition에 bulletImpact 프리팹 생성 -> 유니티가 어떤 것을 위에 렌더링할지 결정하지 못해 이상한 지지직 효과 ->  hitPosition + hitNormal * 0.001f
            //  Quaternion.LookRotation(hitNormal, Vector3.up): quaternion which is aligned to the normal with Vector3.up being upwards
            // * bulletImpactPrefab.transform.rotation: 오브젝트에 가려져서 안 보여서 * -180도

            //10초 후에 자동으로 bulletImpact 파괴
            Destroy(bulletImpactObj, 10f); 

            bulletImpactObj.transform.SetParent(colliders[0].transform);
            //먼소리야?????????????? 13강 8~9분 다시 듣기 

        }
    }

}
