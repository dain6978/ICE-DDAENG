using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UIView
{
    // Show and Hide
    bool isActive = false;

    public void Show()
    {
        gameObject.SetActive(true);
        isActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
}
