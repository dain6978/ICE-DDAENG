using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingAnimation : MonoBehaviour
{
    public Animator dancingAnimator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dancingAnimator.SetTrigger("Dance1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dancingAnimator.SetTrigger("Dance2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            dancingAnimator.SetTrigger("Dance3");
        }
    }
}
