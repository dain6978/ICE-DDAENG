using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;


    PhotonView PV;

    public float fireRate;
    float fireTimer;

    public Animator anim;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
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
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //��ũ��(1��Ī ī�޶�)�� �߾����κ��� ���� ������ ray
        ray.origin = cam.transform.position; // ray�� �������� ī�޶��� ��ġ��

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage, ((GunInfo)itemInfo).isSnow);
            // '?' ǥ��: ���� ������Ʈ�� IDamageable �������̽��� ������ ���� ���� TakeDamage �Լ��� ȣ��ȴٴ� �ǹ�
            // SingleShotGun ��ũ��Ʈ (Ŭ����)�� Gun�� Item Ŭ������ ��ӹ��� -> Item Ŭ������ public ������ itemInfo�� ���� ����
            // (itemInfo�� �ν��Ͻ��� ���� ItemInfo Ŭ�������� �������� �̸��� ���� ������ ���� itemName ���� ����)
            // damaga�� ItemInfo�� ��ӹ��� GunInfo�� �����̱� ������ GunInfo�� ����ȯ (�𸮾� ��������Ʈ Ŭ���� ����ȯ �����ϸ� �ɵ�)

            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal); //��� Ŭ���̾�Ʈ�� ���� hit(raycast�� ��ȯ��)�� ��ġ�� ���ڷ� �ϴ� RPC_Shoot �Լ� ȣ��
            fireTimer = 0.0f;
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
            //  hitPosition�� bulletImpact ������ ���� -> ����Ƽ�� � ���� ���� ���������� �������� ���� �̻��� ������ ȿ�� ->  hitPosition + hitNormal * 0.001f
            //  Quaternion.LookRotation(hitNormal, Vector3.up): quaternion which is aligned to the normal with Vector3.up being upwards
            // * bulletImpactPrefab.transform.rotation: ������Ʈ�� �������� �� ������ * -180��

            //10�� �Ŀ� �ڵ����� bulletImpact �ı�
            Destroy(bulletImpactObj, 10f); 

            bulletImpactObj.transform.SetParent(colliders[0].transform);
            //�ռҸ���?????????????? 13�� 8~9�� �ٽ� ��� 

        }
    }

}