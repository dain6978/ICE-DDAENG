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
            // ��, �ٸ� window (QuitGame, Setting, Help) �� �ƹ��͵� �� ���� ���� ���� �ѱ�
            // (���� ���� �ִ� windodw�� ���� esc ���� ������ �ݰ� �Ѱ� �ϴ� ��... �ϴ� ���߿� �ʿ��ϸ� ����...�Ф�)
            foreach (UIView window in windows) {
                if (window.GetIsActive())
                    return;
            }
            managingWindow.Show();
            mouseCursor.OnCursor();
        }
    }

}
