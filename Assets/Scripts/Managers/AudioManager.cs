using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoSingleton<AudioManager>
{
    AudioSource audioSource;
    
    protected override void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip sfx)
    {
        audioSource.PlayOneShot(sfx);
    }
}
