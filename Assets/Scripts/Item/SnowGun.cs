using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGun : Gun
{
    [SerializeField] Camera cam;
    int playerLayer = (1 << 8);

    PhotonView PV;
    PlayerAnimManager playerAnimManager;

    public float fireRate;
    float fireTimer;

    public Animator anim;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerAnimManager = FindObjectOfType<PlayerAnimManager>();
    }

    public override void Use()
    {
        Shoot();
    }


    private void Update()
    {
        if(fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }
    void Shoot()
    {
        if(fireTimer < fireRate)
        {
            return;
        }

        //raycast 
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //스크린(1인칭 카메라)의 중앙으로부터 뻗어 나오는 ray
        ray.origin = cam.transform.position; // ray의 시작점을 카메라의 위치로
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50.0f, (1 << 0) | (1 << 8) | (1<<11))) // 거리 제한 필요? 총알. 두께도 보기 
        {
            hit.collider.gameObject.GetComponentInParent<IDamageable>()?.TakeSnow();
            //Debug.Log(hit.collider.gameObject);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal); //모든 클라이언트에 대해 hit(raycast의 반환값)의 위치를 인자로 하는 RPC_Shoot 함수 호출
            fireTimer = 0.0f;
            playerAnimManager.ShootAnim();
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


        }
    }

}
