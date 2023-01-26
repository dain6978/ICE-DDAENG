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
        //SpawnManager (���� ������Ʈ)�� �ڽ����� �ִ� Spawnpoint�� (���� ������Ʈ)���� Spawnpoint ��ũ��Ʈ(������Ʈ)�� �����ͼ�,
        //Spawnpoint �迭 spawnpoints�� �Ҵ� (�迭�̴ϱ� GetComponent's'��!)
    }

    public Transform GetSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform; 
        //spawnpoint�� �� �ϳ��� ��ġ �������� ��ȯ
    }
}
