using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_AttackState:StateTemplate<Enemy>
{
    private Transform player;
    


    public Default_AttackState(int id,Enemy o):base(id,o)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        
        player = owner.target.transform;
        owner.enemyAI.speed_anim = 1f;
        owner.SetAITarget(player);
        owner.enemyAI.StartMove();
    }
    public override void OnStay()
    {
        base.OnStay();
        if (player)
        {
            bool inArea = Vector2.Distance(owner.transform.position, player.position) > owner.fSMcreator.attack_radius ? false : true;

            if (!inArea)
            {
                if (!owner.enemyAI.isActiveAndEnabled)
                {
                    owner.OpenAI();
                }
            }
            else
            {
                //攻击
                owner.CloseAI();
                float x = player.position.x - owner.transform.position.x;
                x = x > 0 ? -1 : 1;
                owner.GFX.localScale = new Vector3(x, 1, 1);

                CallAttack();
            }
        }
        else
            machine.TranslateToState(0);
    }
    public override void OnExit()
    {
        base.OnExit();
        player = null;
    }

    public virtual void Attack()
    {

    }
    public bool CallAttack()
    {
        int check = owner.fSMcreator.timing.AttackCheck();
        switch (check)
        {
            case 0:
                break;
            case 1:
                Attack();
                return true;
                break;
            case 2:
                break;
            case 3:
                if (machine.isplayer)
                {
                    machine.player.PlayAnima("reload");
                }
                else
                {
                    owner.PlayAnima("reload");
                }
                //开始换弹动画
                break;
        }
        return false;
    }
}

//攻击计时器类
public class AttackTiming
{
    private int bulletNum = 0;//弹匣容量，默认为0（近战）
    private float shotReload = 0f;//换弹时间
    private float attackSpace = 2f;//攻击间隔
    private float t = 0f;
    private int curBullet = 0;

    //
    private bool isReloading = false;
    public AttackTiming(int bulletNum,float shotReload,float attackSpace)
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
}

