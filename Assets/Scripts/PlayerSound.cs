using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioClip[] footstepSounds_Walk;
    public AudioClip[] footstepSounds_Run;

    public AudioClip landSound;
    public AudioClip shootSound;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayFootStep_Walk()
    {
        if (footstepSounds_Walk != null)
            source.PlayOneShot(footstepSounds_Walk[Random.Range(0, footstepSounds_Walk.Length)]);
    }

    public void PlayFootStep_Run()
    {
        if (footstepSounds_Run != null)
            source.PlayOneShot(footstepSounds_Run[Random.Range(0, footstepSounds_Run.Length)]);
    }

    public void PlayLandSound()
    {
        source.PlayOneShot(landSound);
    }

    public void PlayShootound()
    {
        source.PlayOneShot(shootSound);
    }

}