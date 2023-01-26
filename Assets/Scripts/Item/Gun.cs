using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    //manages recoil calculation and ammo consumption

    // 모든 Gun에 대해 총알 효과
    public GameObject bulletImpactPrefab;

    public abstract override void Use();
}
