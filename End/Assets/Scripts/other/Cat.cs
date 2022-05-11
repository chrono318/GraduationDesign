using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public void AAAPlaySound()
    {
        SoundManager.instance.PlaySoundClipRandom(0,5,SoundManager.instance.effectSound);
    }
}
