using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingManager : MonoBehaviour
{
    public enum ice {ice1, ice2, ice3, ice4};
    public ice currnetIce;
    
    private int count = 0;
    private GameObject brokenIce;


    private void Start()
    {
        brokenIce = this.transform.GetChild(0).gameObject;
        Debug.Log(brokenIce.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        StartCoroutine(CheckIce());
    }

    IEnumerator CheckIce()
    {
        yield return new WaitForSeconds(1f);
        
        count++;
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
        Debug.Log("############�극��ũ!#######");

        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;

        brokenIce.SetActive(true);

        Destroy(gameObject, 3);
    }
}
