using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestarRandom : MonoBehaviour
{
    ParticleSystem system;
    public float CD;
    float currCD;
    public float randomX, randomY;
    Vector2 InitPos;

    void Start()
    {
        system = GetComponent<ParticleSystem>();
        currCD = CD;
        InitPos = transform.position;
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
            if (currCD <= 0)
            {
                currCD = CD;
                float rangeX = Random.Range(InitPos.x - randomX, InitPos.x + randomX);
                float rangeY = Random.Range(InitPos.y - randomY, InitPos.y + randomY);
                transform.position = new Vector3(rangeX, rangeY, transform.position.z);
                system.Play();
            }
        }
    }
}
