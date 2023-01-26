using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField] GameObject graphics;

    private void Awake()
    {
        graphics.SetActive(false);
        // 게임 시작시 모든 spawn 지점 hide (플레이어의 눈에 안 보이게)
    }
}
