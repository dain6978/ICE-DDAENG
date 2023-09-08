using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject aimUI;


    public void Hide()
    {
        playerUI.SetActive(false);
    }

    public void Show()
    {
        playerUI.SetActive(true);
    }

    public void HideAim()
    {
        aimUI.SetActive(false);
    }

    public void ShowAim()
    {
        aimUI.SetActive(true);
    }


    public void Destroy()
    {
        Destroy(playerUI);
    }
}
