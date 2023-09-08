using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject managingWindow;

    private PlayerManager playerManager;
    private PlayerUI playerUI;
    private PhotonView PV;

    private MouseCursor mouseCursor;
    private Stack<GameObject> popupStack;


    //왜 없다 하지... playerController에선 되는데...
    private void Awake()
    {
        //playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        mouseCursor = GetComponent<MouseCursor>();
        popupStack = new Stack<GameObject>();

        playerUI = playerManager.GetPlayerController().GetComponentInChildren<PlayerUI>();
    }


    void Update()
    {
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


    public void SwitchMangingWindow()
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

}
