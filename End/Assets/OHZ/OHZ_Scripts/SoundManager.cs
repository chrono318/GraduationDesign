using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource_Effect,audioSource_Environment;
    public static SoundManager instance;
    public AudioClip[] CombatSound;
    public AudioClip[] UISound;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
