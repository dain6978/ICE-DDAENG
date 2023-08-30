using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UIView : MonoBehaviour
{
    // Alpha
    public float GetAlphaValue()
    {
        return GetComponent<CanvasGroup>().alpha;
    }

    
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
        Image image = getImage();
        if (image != null)
        {
            image.sprite = updateImg;
        }
    }

    public void UpdateTextMeshPro(string updateText)
    {
        string text = getTextMeshPro();
        if (text != null)
        {
            text = updateText;
        }
    }

    // Get Vaule
    public string getTextMeshPro()
    {
        return GetComponent<TextMeshPro>().text;
    }

    public Image getImage()
    {
        return GetComponent<Image>();
    }


}
