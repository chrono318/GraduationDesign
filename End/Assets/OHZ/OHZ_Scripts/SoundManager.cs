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
    /// 播放音频,一次(不受暂停，停止影响）
    /// </summary>
    public void PlaySoundClip(AudioClip clips)
    {
        audioSource_Effect.PlayOneShot(clips);
    }

    /// <summary>
    /// 播放Boss音频,一次(不受暂停，停止影响）
    /// </summary>
    public void PlayBossClip(AudioClip clips)
    {
        bossSource.PlayOneShot(clips);
    }

    /// <summary>
    /// 随机播放某几个音频
    /// </summary>
    /// <param name="randomMin"></param>
    /// <param name="randomMax"></param>
    public void PlaySoundClipRandom(int randomMin, int randomMax,AudioClip[] audioClip)
    {
        int random = Random.Range(randomMin, randomMax);
        audioSource_Effect.PlayOneShot(audioClip[random]);
    }

    public AudioClip GetSoundClipRandom(int randomMin, int randomMax, AudioClip[] audioClip)
    {
        int random = Random.Range(randomMin, randomMax);
        return audioClip[random];
    }

    /// <summary>
    /// 随机播放某几个音频,Boss音频管理器
    /// </summary>
    /// <param name="randomMin"></param>
    /// <param name="randomMax"></param>
    public void PlayBossClipRandom(int randomMin, int randomMax, AudioClip[] audioClip)
    {
        int random = Random.Range(randomMin, randomMax);
        bossSource.PlayOneShot(audioClip[random]);
    }

    /// <summary>
    /// 强制停止音频管理器
    /// </summary>
    /// <param name="clips"></param>
    public void StopSound(AudioClip clips)
    {
        audioSource_Effect.Stop();
    }

    /// <summary>
    /// 强制停止Boss音频管理器
    /// </summary>
    /// <param name="clips"></param>
    public void StopBossSound(AudioClip clips)
    {
        bossSource.Stop();
    }

    /// <summary>
    /// 暂停音频管理器
    /// </summary>
    /// <param name="clips"></param>
    public void PauseSound(AudioClip clips)
    {
        audioSource_Effect.Pause();
    }

    /// <summary>
    /// 暂停音频管理器
    /// </summary>
    /// <param name="clips"></param>
    public void PauseBossSound(AudioClip clips)
    {
        bossSource.Pause();
    }

}
