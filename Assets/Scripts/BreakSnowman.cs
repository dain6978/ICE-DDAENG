using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSnowman : MonoBehaviour
{
    Collider[] colliders;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();

        //foreach(Collider c in colliders)
        //{
        //    c.gameObject.GetComponent<Renderer>().enabled = false;
        //    Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
        //    rb.constraints = (RigidbodyConstraints)126;
        //}
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Break();
        }
    }

    public void Break()
    {
        gameObject.SetActive(true);
        //foreach (Collider c in colliders)
        //{
        //    c.gameObject.GetComponent<Renderer>().enabled = true;
        //    Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
        //    rb.constraints = (RigidbodyConstraints)0;
        //}
    }
}
