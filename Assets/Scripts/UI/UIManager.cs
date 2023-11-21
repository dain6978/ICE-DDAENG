using Photon.Pun;
using Photon.Realtime;
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


    private MouseCursor mouseCursor;
    private Stack<GameObject> popupStack;

    private GameObject currentPlayer;
    private PlayerUI currentPlayerUI;

    public float sec = 0f;
    public int min = 0;


    private void Start()
    {
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

        if (popupStack.Count == 1)
        {
            mouseCursor.OffCursor();
            ShowPlayerUI();
        }

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
            HidePlayerUI();

            ShowPopupUI(managingWindow);
            mouseCursor.OnCursor();
        }
        else
        {
            HidePopupUI();
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

    public void DestroyGameUI()
    {
        foreach (Transform child in gameCanvas.transform)
        {
            Destroy(child.gameObject);
        }
    }


    void ShowPlayerUI()
    {
        currentPlayerUI.Show();
        currentPlayer.GetComponent<PlayerController>().canClick = true; // 플레이어 입력 활성화
    }

    void HidePlayerUI()
    {
        SetPlayerUI();
        currentPlayerUI.Hide();
        currentPlayer.GetComponent<PlayerController>().canClick = false; // 플레이어 입력 비활성화
    }

    void SetPlayerUI()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.IsLocal)
            {
                currentPlayer = RoomManager.Instance.playerDict[player].playerController;
                currentPlayerUI = currentPlayer.GetComponentInChildren<PlayerUI>();
            }
        }
    }
}
