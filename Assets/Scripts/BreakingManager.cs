using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingManager : MonoBehaviour
{
    [SerializeField] enum ice {ice1, ice2, ice3, ice4};
    [SerializeField] Material[] materials;
    [SerializeField] ice currnetIce;

    private bool check = false;
    private int count = 0;
    private GameObject brokenIce;

    private void Start()
    {
        switch (currnetIce)
        {
            case ice.ice1:
                count = 1;
                break;
            case ice.ice2:
                count = 2;
                break;
            case ice.ice3:
                count = 3;
                break;
            case ice.ice4:
                count = 4;
                break;
        }

        brokenIce = this.transform.GetChild(0).gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool check = false;
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (!check)
            StartCoroutine(CheckIce());
    }

    IEnumerator CheckIce()
    {
        yield return new WaitForSeconds(1f);
        check = true;

        count--;
        if (count == 0) 
            BreakIce();
        else
            SetMaterial(count);

        yield return null;
    }

    void BreakIce()
    {
        AudioManager.Instacne.PlaySFX("Destruction_die");

        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;

        brokenIce.SetActive(true);

        Destroy(gameObject, 3);
    }

    void SetMaterial(int count)
    {
        this.gameObject.GetComponent<MeshRenderer>().material =  materials[count-1];
    }
}
