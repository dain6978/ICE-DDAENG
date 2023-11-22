using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject playerCanvas;
    [SerializeField] GameObject aimUI;
    [SerializeField] GameObject iceUI;
    [SerializeField] GameObject respawnUI;
    [SerializeField] Sprite[] iceState;
    [SerializeField] Image[] iceImg;
    private Image respawnGauge;
    private bool isDeath;
    
    private void Start()
    {
        respawnGauge = respawnUI.transform.GetChild(2).gameObject.GetComponent<Image>();
        respawnGauge.fillAmount = 0;
    }

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

    public void HideIce()
    {
        iceUI.SetActive(false);
    }

    public void ShowRespawn()
    {
        respawnUI.SetActive(true);
        isDeath = true;
    }

    private void Update()
    {
        if (isDeath)
        {
            respawnGauge.fillAmount += (Time.deltaTime/2.8f);
        }
    }

    public void AddIce()
    {
        for (int i = 0; i < iceImg.Length; i++)
        {
            // 만약 i번째 ice UI가 non-iced 상태라면 ice 상태로 바꾸고 alpha값 변경
            if (iceImg[i].sprite == iceState[0])
            {
                iceImg[i].sprite = iceState[1];
                Color color = iceImg[i].color;
                color.a = 1.0f;
                iceImg[i].color = color;

                return;
            }
        }
    }

    public void ResetIce()
    {
        for (int i = 0; i < iceImg.Length; i++)
        {
            iceImg[i].sprite = iceState[0];
            Color color = iceImg[i].color;
            color.a = 0.4f;
            iceImg[i].color = color;
        }
    }
}
