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


    private GameManager gameManager;
    private MouseCursor mouseCursor;
    private Stack<GameObject> popupStack;
    private PlayerUI currentPlayerUI;

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
            mouseCursor.OffCursor();

            ShowPlayerUI();
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
        //SetPlayerUI();
        Debug.Log("쇼");
        currentPlayerUI.Show();
    }

    void HidePlayerUI()
    {
        SetPlayerUI();
        Debug.Log("하이드");
        currentPlayerUI.Hide();
    }

    void SetPlayerUI()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.IsLocal)
                currentPlayerUI = RoomManager.Instance.playerDict[player].playerController.GetComponentInChildren<PlayerUI>();
        }
    }
}
