﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunken : MonoBehaviour
{
    public bool isUp = false;
    GameObject player;
    Collider2D coll;
    /// <summary>
    /// 维持凸起状态时间
    /// </summary>
    public float keepUpTime = 2f;
    float currKeepUpTime;
    /// <summary>
    /// 维持缩地状态时间
    /// </summary>
    public float keepDownTime = 2f;
    float currkeepDownTime;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        coll = GetComponent<BoxCollider2D>();
        currkeepDownTime = keepDownTime;
        currKeepUpTime = keepUpTime;
    }

    void Update()
    {
        KeepUpTime();
        KeepDownTime();
    }

    /// <summary>
    /// 保持凸起时间
    /// </summary>
    public void KeepUpTime()
    {
        if (isUp)
        {
            coll.enabled = true;
            currKeepUpTime -= Time.deltaTime;
            if(currKeepUpTime <= 0)
            {
                isUp = false;
                currKeepUpTime = keepUpTime;
            }
        }
    }

    /// <summary>
    /// 保持缩地状态
    /// </summary>
    public void KeepDownTime()
    {
        if (!isUp)
        {
            coll.enabled = false;
            currkeepDownTime -= Time.deltaTime;
            if (currkeepDownTime <= 0)
            {
                isUp = true;
                currkeepDownTime = keepDownTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
                print("受伤");
                //#对接#受伤#对接#
        }
    }
}
