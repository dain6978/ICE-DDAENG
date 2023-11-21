using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingManager : MonoBehaviour
{
    public enum ice {ice1, ice2, ice3, ice4};
    public ice currnetIce;

    private bool check = false;
    private int count = 0;
    private GameObject brokenIce;


    private void Start()
    {
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
        
        count++;
        Debug.Log("체크아이스 카운트 증가");
        switch (currnetIce)
        {
            case ice.ice1:
                if (count == 1)
                    BreakIce();
                break;
            case ice.ice2:
                if (count == 2)
                    BreakIce();
                break;
            case ice.ice3:
                if (count == 3)
                    BreakIce();
                break;
            case ice.ice4:
                if (count == 4)
                    BreakIce();
                break;

        }
        yield return null;
    }

    void BreakIce()
    {
        Debug.Log("############브레이크!#######");

        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;

        brokenIce.SetActive(true);

        Destroy(gameObject, 3);
    }
}
