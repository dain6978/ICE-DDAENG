using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSkin : MonoBehaviour
{
    public List<GameObject> SkinList;
    GameObject currentSkin;
    int currentSkinIndex = 0;

    

    private void Start()
    {
        currentSkin = SkinList[currentSkinIndex];
        SkinList[currentSkinIndex].SetActive(true);
    }

    public void NextSkin()
    {
        SkinList[currentSkinIndex].SetActive(false);
        currentSkinIndex++;
        if(currentSkinIndex >= SkinList.Count)
        {
            currentSkinIndex = 0;
        }
        currentSkin = SkinList[currentSkinIndex];
        SkinList[currentSkinIndex].SetActive(true);
    }

    public void BeforeSkin()
    {
        SkinList[currentSkinIndex].SetActive(false);
        SkinList[currentSkinIndex].SetActive(false);
        currentSkinIndex--;
        if (currentSkinIndex < 0 )
        {
            currentSkinIndex = SkinList.Count -1;
        }
        currentSkin = SkinList[currentSkinIndex];
        SkinList[currentSkinIndex].SetActive(true);
    }

    public void SetSkin()
    {
        PlayerPrefs.SetInt("userskin", currentSkinIndex);
        //PlayerSkinManager.Instance.mesh = currentSkin.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        //PlayerSkinManager.Instance.material = currentSkin.GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
    }

}
