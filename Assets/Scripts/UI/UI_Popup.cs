using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UIView
{

    public void Show()
    {
        UIManager.Instance.Push(gameObject);
        gameObject.SetActive(true);
        Debug.Log("Show");
    }


    public void Hide()
    {
        if (UIManager.Instance.GetStackCount() == 0)
        {
            Debug.Log("하이드할수없다");
            return;
        }

        GameObject popup = UIManager.Instance.Pop();
        popup.SetActive(false);
        Debug.Log("Hide");
    }


    public void HideAll()
    {
        while (UIManager.Instance.GetStackCount() > 0)
            Hide();
    }
}
