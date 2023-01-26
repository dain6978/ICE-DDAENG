using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //responsible for actually giving players their spawn points

    public static SpawnManager Instance;

    Spawnpoint[] spawnpoints;

    private void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>(); 
        //SpawnManager (게임 오브젝트)의 자식으로 있는 Spawnpoint들 (게임 오브젝트)에서 Spawnpoint 스크립트(컴포넌트)를 가져와서,
        //Spawnpoint 배열 spawnpoints에 할당 (배열이니까 GetComponent's'로!)
    }

    public Transform GetSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform; 
        //spawnpoint들 중 하나의 위치 랜덤으로 반환
    }
}
