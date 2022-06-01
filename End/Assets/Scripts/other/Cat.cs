using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public AudioSource audioSource;
    public void AAAPlaySound()
    {
        AudioClip audioClip = SoundManager.instance.GetSoundClipRandom(0,5,SoundManager.instance.effectSound);
        audioSource.PlayOneShot(audioClip);
    }
}
