using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    [SerializeField] private UIView managingWindow;
    private MouseCursor mouseCursor;

    private Stack<GameObject> popupStack;
    public int stackCount;


    private void Start()
    {
        mouseCursor = GetComponent<MouseCursor>();
        popupStack = new Stack<GameObject>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            ShowOrHideMangingWindow();
        }

        stackCount = GetStackCount();
    }



    public void Push(GameObject popup)
    {
        popupStack.Push(popup);

        foreach (GameObject go in popupStack)
        {
            Debug.Log($"popup : {go}");
        }
        Debug.Log(popupStack.Count);
    }

    public GameObject Pop()
    {
        Debug.Log("POP");
        GameObject popup = popupStack.Pop();
        //popup = null;
        return popup;
    }

    public int GetStackCount()
    {
        return popupStack.Count;
    }

    public void ShowPopup(GameObject popup)
    {
        Push(popup);
        popup.SetActive(true);

    }


    public void ShowOrHideMangingWindow()
    {
        if (popupStack.Count == 0)
        {
            ShowPopup(managingWindow.gameObject);
            mouseCursor.OnCursor();
        }
        else
        {
            while (popupStack.Count > 0)
            {
                GameObject popup = Pop();
                popup.SetActive(false);
                Debug.Log("Hide" + popup + "Window");
            }
            mouseCursor.OffCursor();
        }
    }

}
