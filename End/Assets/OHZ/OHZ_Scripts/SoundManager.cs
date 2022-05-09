using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource_Effect,audioSource_Environment;
    public static SoundManager soundManager;
    public AudioClip[] CombatSound;
    public AudioClip[] UISound;

    private void Awake()
    {
        if(soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(soundManager);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ≤•∑≈“Ù∆µ
    /// </summary>
    public void PlaySoundClip(AudioClip clips)
    {
        audioSource_Effect.PlayOneShot(clips);
    }
}
