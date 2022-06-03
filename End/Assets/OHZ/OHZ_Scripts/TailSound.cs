using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSound : MonoBehaviour
{
    Vector3 currPos;
    AudioSource audioSource;
    bool stopMove = true;
    bool playOver;

    public float delayTime;
    float currDelayTime;
    public float mul = 0.9f;

    private void Start()
    {
        currPos = transform.position;
        audioSource = transform.GetComponent<AudioSource>();
        currDelayTime = delayTime;
    }

    void Update()
    {
        CheckMove();
    }

    public void CheckMove()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        if(currPos != transform.position)
        {
            audioSource.volume = 1;
            currDelayTime = delayTime;
            currPos = transform.position;
            audioSource.clip = SoundManager.instance.UISound[3];
            if (!audioSource.isPlaying && !playOver)
            {
                audioSource.PlayOneShot(audioSource.clip);
                playOver = true;
            }
            else if (!audioSource.isPlaying && playOver)
            {
                audioSource.clip = SoundManager.instance.UISound[3];
                audioSource.PlayOneShot(audioSource.clip);
                audioSource.loop = true;
            }
        }
        else
        {
            currDelayTime -= Time.deltaTime;
            audioSource.volume *= mul;
            if (currDelayTime <= 0)
            {
                audioSource.Stop();
            }
            //audioSource.loop = false;
            if (!audioSource.isPlaying)
                playOver = false;
        }
    }
}
