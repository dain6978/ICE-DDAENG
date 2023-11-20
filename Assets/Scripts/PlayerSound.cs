using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioClip[] footstepSounds_Walk;
    public AudioClip[] footstepSounds_Run;

    public AudioClip landSound;

    AudioSource source;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine) return;
        source = GetComponent<AudioSource>();
    }
    public void PlayFootStep_Walk()
    {
        if (!PV.IsMine) return;
        if (footstepSounds_Walk != null)
            source.PlayOneShot(footstepSounds_Walk[Random.Range(0, footstepSounds_Walk.Length)]);
    }

    public void PlayFootStep_Run()
    {
        if (!PV.IsMine) return;
        if (footstepSounds_Run != null)
            source.PlayOneShot(footstepSounds_Run[Random.Range(0, footstepSounds_Run.Length)]);
    }

    public void PlayLandSound()
    {
        if (!PV.IsMine) return;
        source.PlayOneShot(landSound);
    }
}