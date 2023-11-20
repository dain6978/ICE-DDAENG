using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCanUse : MonoBehaviour
{

    public DamageGun damGun;
    private void OnEnable()
    {
        damGun.canUse = false;
        Invoke("ShootAble", 0.8f);
    }

    private void OnDisable()
    {
        damGun.canUse = true;
    }

    void ShootAble()
    {
        damGun.canUse = true;
    }
}
