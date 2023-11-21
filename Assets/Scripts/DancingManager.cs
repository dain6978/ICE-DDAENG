using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DancingManager : MonoBehaviour
{
    GameManager gameManager;
    DancingAnimation dancingAnimaton;
    PlayerController playerController;
    
    public RuntimeAnimatorController dancingAnimator;
    Animator playerAnimator;
    List<GameObject> playerLists;

    // item 
    private GameObject items;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
    }

    public void SetAnimator(GameObject player)
    {
        if (player != null) // if (player != null)
        {
            Destroy(player.GetComponent<PlayerAnimManager>());
            player.GetComponent<PhotonAnimatorView>().enabled = false;

            player.GetComponent<Animator>().runtimeAnimatorController = dancingAnimator;
            player.AddComponent<DancingAnimation>();
            dancingAnimaton = player.GetComponent<DancingAnimation>();
            dancingAnimaton.dancingAnimator = player.GetComponent<Animator>();

            playerController = player.GetComponent<PlayerController>();
            items = playerController.GetItems();
            items.SetActive(false);
        }
    }
}
