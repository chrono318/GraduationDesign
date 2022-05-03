using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ʱ����
public class AttackTimer
{
    private int bulletNum = 0;//��ϻ������Ĭ��Ϊ0����ս��
    private float shotReload = 0f;//����ʱ��
    private float attackSpace = 2f;//�������
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
    /// 1�����Թ���       2�����������      0��������      3:��ʼ��������
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
            }//�ڻ���
            if (curBullet >= bulletNum)
            {
                isReloading = true;
                t = 0;
                return 3;
            }//�û�����
            if (t < attackSpace)
            {
                return 2;
            }//���������

            //
            t = 0;
            curBullet++;
            return 1;
        }
    }

    /// <summary>
    /// ����Ǹ����������ǹ
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

