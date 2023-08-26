using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIView : MonoBehaviour
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

    public void ShowOrHide()
    {
        isActive = GetIsActive();

        if (!isActive)
            Show();
        else
            Hide();
    }

    public bool GetIsActive()
    {
        return isActive;
    }


    // Alpha
    public float GetAlphaValue()
    {
        return GetComponent<CanvasGroup>().alpha;
    }

    //  나중에 제대로...
    //public void FadeOut(float fadingSpeed)
    //{
    //    StartCoroutine(Fading(1, 0, fadingSpeed));
    //}

    //public void FadeIn(float fadingSpeed)
    //{
    //    StartCoroutine(Fading(0, 1, fadingSpeed));
    //}

    //IEnumerator Fading(float start, float end, float speed)
    //{
    //    float counter = 0f;
    //    while (counter < 0.4f)
    //    {
    //        counter += Time.deltaTime * speed;
    //        GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, counter / 0.4f);
    //        yield return null;
    //    }
    //}


    // Update Value
    public void UpdateImage(Sprite updateImg)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.sprite = updateImg;
        }
    }

    public void UpdateTextMeshPro(string updateText)
    {
        TextMeshPro text = GetComponent<TextMeshPro>();
        if (text != null)
        {
            text.text = updateText;
        }
    }

    // Get Vaule
    public string getTextMeshProInputField()
    {
        return GetComponent<TMP_InputField>().text;
    }
    public string getTextMeshPro()
    {
        return GetComponent<TextMeshPro>().text;
    }
    public int getTextMeshProLength()
    {
        return GetComponent<TextMeshPro>().text.Length;
    }


}
