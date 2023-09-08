using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject managingWindow;
    [SerializeField] private GameObject aimUI;

    private MouseCursor mouseCursor;

    private Stack<GameObject> popupStack;


    private void Start()
    {
        mouseCursor = GetComponent<MouseCursor>();
        popupStack = new Stack<GameObject>();
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
            ShowPopupUI(managingWindow);
            mouseCursor.OnCursor();
        }
        else
        {
            HidePopupUI();
            mouseCursor.OffCursor();
        }
    }

}
