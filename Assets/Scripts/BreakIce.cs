using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakIce : MonoBehaviour
{
    [SerializeField] GameObject ice1, ice2, ice3;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(IceSetactive());
            GetComponent<MeshCollider>().enabled = false;
        }
    }

    IEnumerator IceSetactive()
    {
        yield return new WaitForSeconds(1f);
        ice2.SetActive(true);
        ice1.SetActive(false);

        yield return new WaitForSeconds(1f);
        ice3.SetActive(true);
        ice2.SetActive(false);

    }
}
