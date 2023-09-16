using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject managingWindow;
    [SerializeField] private TextMeshProUGUI timerText;

    private GameObject player;
    private PlayerUI playerUI;
    private PhotonView PV;

    private GameManager gameManager;
    private MouseCursor mouseCursor;
    private Stack<GameObject> popupStack;



    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        mouseCursor = GetComponent<MouseCursor>();
        popupStack = new Stack<GameObject>();

        // player 가져오는 법
        //player = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
    }


    void Update()
    {
        updateTimer();

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SwitchMangingWindow();
        }
    }

    public void SetPlayer(GameObject playerController)
    {
        player = playerController;

        if (player != null)
            playerUI = player.GetComponentInChildren<PlayerUI>();
    }

    // scene UI
    public void ShowSceneUI(GameObject scene)
    {
        scene.SetActive(true);
    }

    public void HideSceneUI(GameObject scene)
    {
        scene.SetActive(false);
    }


    // pop up UI
    public void ShowPopupUI(GameObject popup)
    {
        popupStack.Push(popup);
        popup.SetActive(true);
    }

    public void HidePopupUI()
    {
        if (popupStack.Count == 0)
            return; 

        GameObject popup = popupStack.Pop();
        popup.SetActive(false);
        popup = null;
    }

    public void HideAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            HidePopupUI();
        }
    }


    private void SwitchMangingWindow()
    {
        if (popupStack.Count == 0)
        {
            if (playerUI != null)
                playerUI.HideAim();

            ShowPopupUI(managingWindow);
            mouseCursor.OnCursor();
        }
        else
        {
            HidePopupUI();
            mouseCursor.OffCursor();

            if (playerUI != null)
                playerUI.ShowAim();
        }
    }

    private void updateTimer()
    {
        float time = gameManager.GetGameTime();
        timerText.text = Convert.ToString(time);
    }
}
