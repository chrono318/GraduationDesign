using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource_Effect,bossSource;
    public static SoundManager instance;
    public AudioClip[] combatSound;
    public AudioClip[] UISound;
    public AudioClip[] effectSound;
    public AudioClip[] BossSound;
    public AudioClip[] loopSound;

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
    /// 播放音频,一次
    /// </summary>
    public void PlaySoundClip(AudioClip clips)
    {
        audioSource_Effect.PlayOneShot(clips);
    }

    /// <summary>
    /// 播放Boss音频,一次
    /// </summary>
    public void PlayBossSource(AudioClip clips)
    {
        bossSource.PlayOneShot(clips);
    }
}
