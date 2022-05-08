using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStart : MonoBehaviour
{
    ParticleSystem system;
    public float CD;
    float currCD;


    void Start()
    {
        system = GetComponent<ParticleSystem>();
        currCD = CD;
    }

    void Update()
    {
        Replay();
    }

    /// <summary>
    /// ÖØÐÂ²¥·Å
    /// </summary>
    public void Replay()
    {
        if (system.isStopped)
        {
            currCD -= Time.deltaTime;
            if(currCD <= 0)
            {
                currCD = CD;
                system.Play();
            }
        }
    }
}
