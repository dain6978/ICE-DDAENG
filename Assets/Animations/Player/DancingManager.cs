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

    public void SetAnimator(Player player)
    {
        Debug.Log("set animator 호출");
        GameObject playerObject = RoomManager.Instance.playerDict[player].playerController;

        if (playerObject != null) // if (player != null)
        {
            Destroy(playerObject.GetComponent<PlayerAnimManager>());
            playerObject.GetComponent<Animator>().runtimeAnimatorController = dancingAnimator;
            playerObject.AddComponent<DancingAnimation>();
            dancingAnimaton = playerObject.GetComponent<DancingAnimation>();
            dancingAnimaton.dancingAnimator = playerObject.GetComponent<Animator>();

            playerController = playerObject.GetComponent<PlayerController>();
            items = playerController.GetItems();
            items.SetActive(false);
        }
        else
        {
            Debug.Log("player object is null");
        }
    }

    //public void SetPlayer(GameObject playerController)
    //{
    //    player = playerController;
    //    if (player != null)
    //    {
    //        playerAnimator = player.GetComponent<Animator>();
    //        Debug.Log("SetPlayer");
    //    }
    //}


}
