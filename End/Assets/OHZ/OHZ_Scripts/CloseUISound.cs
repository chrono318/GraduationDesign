using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUISound : MonoBehaviour
{
    
    public void PlayCloseSound()
    {
        SoundManager.instance.PlaySoundClip(SoundManager.instance.UISound[0]);
    }
}
