using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject playerCanvas;
    [SerializeField] GameObject aimUI;
    [SerializeField] Sprite[] iceState;
    [SerializeField] Image[] iceUI;

    public void Hide()
    {
        playerCanvas.SetActive(false);
    }

    public void Show()
    {
        playerCanvas.SetActive(true);
    }

    public void HideAim()
    {
        aimUI.SetActive(false);
    }

    public void ShowAim()
    {
        aimUI.SetActive(true);
    }


    public void AddIce()
    {
        for (int i = 0; i < iceUI.Length; i++)
        {
            // 만약 i번째 ice UI가 non-iced 상태라면 ice 상태로 바꾸고 alpha값 변경
            if (iceUI[i].sprite == iceState[0])
            {
                iceUI[i].sprite = iceState[1];
                Color color = iceUI[i].color;
                color.a = 1.0f;
                iceUI[i].color = color;

                return;
            }
        }
    }

    public void ResetIce()
    {
        for (int i = 0; i < iceUI.Length; i++)
        {
            iceUI[i].sprite = iceState[0];
            Color color = iceUI[i].color;
            color.a = 0.4f;
            iceUI[i].color = color;
        }
    }
}
