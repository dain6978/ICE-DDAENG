using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    //manages recoil calculation and ammo consumption

    // ��� Gun�� ���� �Ѿ� ȿ��
    public GameObject bulletImpactPrefab;

    public abstract override void Use();
}
