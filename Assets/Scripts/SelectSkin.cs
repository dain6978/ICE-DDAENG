using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SelectSkin : MonoBehaviourPunCallbacks
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
        Hashtable hash = new Hashtable();
        hash.Add("skinIndex", currentSkinIndex);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

}
