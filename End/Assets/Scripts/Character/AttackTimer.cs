using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击计时器类
public class AttackTimer
{
    private int bulletNum = 0;//弹匣容量，默认为0（近战）
    private float shotReload = 0f;//换弹时间
    private float attackSpace = 2f;//攻击间隔
    private float t = 0f;
    private int curBullet = 0;

    //
    private bool isReloading = false;
    public AttackTimer(int bulletNum, float shotReload, float attackSpace)
    {
        this.bulletNum = bulletNum;
        this.shotReload = shotReload;
        this.attackSpace = attackSpace;
        this.t = attackSpace;
    }
    public void Update()
    {
        t += Time.deltaTime;
    }
    /// <summary>
    /// 1：可以攻击       2：攻击间隔中      0：换弹中      3:开始换弹动画
    /// </summary>
    /// <returns></returns>
    public int AttackCheck()
    {
        if (bulletNum == 0)
        {
            if (t > attackSpace)
            {
                t = 0;
                return 1;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            if (isReloading)
            {
                if (t > shotReload)
                {
                    t = 0;
                    isReloading = false;
                    curBullet = 1;
                    return 1;
                }
                else
                {
                    return 0;
                }
            }//在换弹
            if (curBullet >= bulletNum)
            {
                isReloading = true;
                t = 0;
                return 3;
            }//该换弹了
            if (t < attackSpace)
            {
                return 2;
            }//攻击间隔中

            //
            t = 0;
            curBullet++;
            return 1;
        }
    }

    /// <summary>
    /// 针对那个连续发射的枪
    /// </summary>
    /// <param name="time"></param>
    public void SetAttackSpace(float time)
    {
        this.attackSpace = time;
    }
    public void SetReloadTime(float t)
    {
        this.shotReload = t;
    }
}

