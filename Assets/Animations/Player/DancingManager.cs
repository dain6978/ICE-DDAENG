using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingManager : MonoBehaviour
{
    GameObject player;
    GameManager gameManager;
    DancingAnimation dancingAnimaton;
    PlayerController playerController;
    
    public RuntimeAnimatorController dancingAnimator;
    Animator playerAnimator;

    // item ����
    private GameObject items;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
    }
    private void Update()
    {
        if (gameManager.isEnd)
        {
            if (player != null)
            { 
                playerAnimator.runtimeAnimatorController = dancingAnimator;
                player.AddComponent<DancingAnimation>();
                dancingAnimaton = player.GetComponent<DancingAnimation>();
                dancingAnimaton.dancingAnimator = player.GetComponent<Animator>();

                playerController = player.GetComponent<PlayerController>();
                items = playerController.GetItems();
                items.SetActive(false);

                Destroy(this);

            }
            else
            {
                Debug.Log("player is null");
            }
        }
    }

    public void SetPlayer(GameObject playerController)
    {
        player = playerController;
        playerAnimator = player.GetComponent<Animator>();
        Debug.Log("SetPlayer");
    }

    
}
