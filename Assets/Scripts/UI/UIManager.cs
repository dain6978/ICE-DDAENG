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
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private TextMeshProUGUI timerText;

    private GameObject player;
    private PlayerUI playerUI;
    private PhotonView PV;

    private GameManager gameManager;
    private MouseCursor mouseCursor;
    private Stack<GameObject> popupStack;

    public float sec = 0f;
    public int min = 0;


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
        // 게임 종료
        if (gameManager.isEnd)
        {
            foreach (Transform child in gameCanvas.transform)
                Destroy(child.gameObject); // 게임 씬 캔버스 파괴 
            playerUI.Destroy(); // 플레이어 캔버스 파괴
            Destroy(this); 
        }

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
        sec += Time.deltaTime;
        if (sec >= 60f)
        {
            min += 1;
            sec = 0;
        }
        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
    }
}
