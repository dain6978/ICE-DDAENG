using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //[Header ("Window")]
    [SerializeField] private UIView managingWindow;
    [SerializeField] private UIView helpWindow;
    [SerializeField] private UIView settingWindow;
    [SerializeField] private UIView quitGameWindow;

    private MouseCursor mouseCursor;
    private UIView[] windows;

    private void Start()
    {
        mouseCursor = GetComponent<MouseCursor>();

        windows = new UIView[] { managingWindow, helpWindow, settingWindow, quitGameWindow };
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            ShowOrHideMangingWindow();
        }
    }

    public void ShowOrHideMangingWindow()
    {
        if (managingWindow.GetIsActive())
        {
            managingWindow.Hide();
            mouseCursor.OffCursor();
        }
        else
        {
            // 단, 다른 window (QuitGame, Setting, Help) 중 아무것도 안 켜져 있을 때만 켜기
            // (현재 켜져 있는 windodw에 따라서 esc 누를 때마다 닫고 켜고 하는 건... 일단 나중에 필요하면 수정...ㅠㅠ)
            foreach (UIView window in windows) {
                if (window.GetIsActive())
                    return;
            }
            managingWindow.Show();
            mouseCursor.OnCursor();
        }
    }

}
