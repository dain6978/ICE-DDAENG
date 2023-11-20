using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowCanUse : MonoBehaviour
{

    public SnowGun snowGun;
    private void OnEnable()
    {
        snowGun.canUse = false;
        Invoke("ShootAble", 0.5f);
    }

    private void OnDisable()
    {
        snowGun.canUse = true;
    }

    void ShootAble()
    {
        snowGun.canUse = true;
    }
}
