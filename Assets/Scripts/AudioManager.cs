using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instacne;

    [SerializeField]
    AudioSource BGM;


    [SerializeField]
    AudioSource SFX;

   
    public AudioClip[] Bgms;

    private void Awake()
    {
        if (Instacne == null)
        {
            Instacne = this;
            DontDestroyOnLoad(Instacne);
        }
        else
            Destroy(gameObject);

        var obj = FindObjectsOfType<AudioManager>();
        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    public void PlayBGM(AudioClip audio)
    {
        BGM.clip = audio;
        BGM.Play();
    }

    public void PlaySFX(AudioClip audio)
    {
        SFX.clip = audio;
        SFX.Play();
    }

    public void StopBGM()
    {
        BGM.Stop();
    }

    public void StopSFX()
    {
        SFX.Stop();
    }
}
