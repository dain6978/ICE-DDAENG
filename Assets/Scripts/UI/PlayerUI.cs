using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject aimUI;
    [SerializeField] Sprite[] iceState;
    [SerializeField] Image[] iceUI;

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

    public void AddIce()
    {
        for (int i = 0; i < iceUI.Length; i++)
        {
            // ���� i��° ice UI�� non-iced ���¶�� ice ���·� �ٲٰ� alpha�� ����
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
