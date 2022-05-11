using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MoveObject
{
    [Header("船锚")]
    //#对接#
    //移动播放移动动画
    //#对接#
    public float damage;

    //技能指示器
    #region
    GameObject indicator;
    public float speed_ohz = 100f;//转向速度
    public float longSpeed = 1f;//伸长速度
    private Vector2 direction;
    public float beilv = 2f;

    SpriteRenderer sr;
    float initSize;
    float currSize;
    public float maxLongX = 100f;//长度限制
    bool reachMaxLong;//是否到达最长
    public float Keeptime = 1.5f;//到达长度限制后，持续时间
    float currkeeptime;//当前持续时间
    bool isUseSkill;//是否在使用技能

    public float attackCD = 1f;
    float currAttackCD;
    bool canAttack;

    float rotateAngle;
    #endregion
    //技能
    public Transform lastPoint;
    bool isRush;
    public float rushSpeed = 40f;
    GameObject ship;
    Animator animator;


    public enum skillIndicator
    {
        默认 = 0,
        复原,
        伸长,
        维持
    }

    public skillIndicator currSkillState;

    public skillIndicator skillState
    {
        get => currSkillState;
        set
        {
            if(currSkillState != value)
            {
                currSkillState = value;
                switch (value)
                {
                    case skillIndicator.默认:
                        break;
                    case skillIndicator.复原:
                        sr.color = new Color(255, 255, 255, 0);
                        ship.transform.rotation = Quaternion.Euler(0, 0, 0);
                        ship.transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        currSize = initSize;
                        reachMaxLong = false;
                        sr.size = new Vector2(currSize, sr.size.y);
                        animator.Play("idle1");
                        break;
                    case skillIndicator.伸长:
                        sr.color = new Color(255, 255, 255, 255);
                        animator.Play("hold up");
                        break;
                    case skillIndicator.维持:
                        sr.color = new Color(255, 255, 255, 255);
                        break;
                }
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        ship = transform.Find("1").gameObject;
        animator = ship.GetComponent<Animator>();
        InitIndicator();
    }

    void Update()
    {
        DefaultUpdate();
        if (!isPlayer) return;
        if (!isRush)
        {
            FollowMouse();
            StateChange();
            Long();
            LongKeep();
            AttackTiming();
        }
        else
        {
            Rush();
        }
    }

    /// <summary>
    /// 技能指示器初始化
    /// </summary>
    public void InitIndicator()
    {
        indicator = transform.Find("SkillIndicator").gameObject;
        sr = indicator.transform.GetComponent<SpriteRenderer>();
        animator.Play("idle1");
        initSize = sr.size.x;
        currSize = initSize;
        currkeeptime = Keeptime;
        canAttack = true;
        currAttackCD = attackCD;
        sr.color = new Color(255, 255, 255, 0);
    }

    /// <summary>
    /// 技能指示器，朝鼠标方向旋转
    /// </summary>
    void FollowMouse()
    {
        //得到物体指向鼠标位置的向量
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - indicator.transform.position;
        //通过反正切函数得到弧度并转化为角度
        rotateAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //指定向屏幕外的z轴为旋转轴并且旋转
        Quaternion rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward);

        Quaternion rotation2 = Quaternion.AngleAxis(rotateAngle + 90, Vector3.forward);

        indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, rotation, speed_ohz * Time.deltaTime);
        if (skillState == skillIndicator.伸长 || skillState == skillIndicator.维持)
        {
            //print(animator.transform.rotation);
            ship.transform.parent.gameObject.transform.rotation = Quaternion.Slerp(ship.transform.parent.gameObject.transform.rotation, rotation2, speed_ohz * Time.deltaTime);
        }
    }

    /// <summary>
    /// 技能指示器状态切换
    /// </summary>
    public void StateChange()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isRush = true;
            SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[29]);
            //skillState = skillIndicator.复原;
            //sr.color = new Color(255, 255, 255, 0);
            canAttack = false;
        }

        if (Input.GetMouseButton(0) && canAttack)
        {
            if (!reachMaxLong)
            {
                skillState = skillIndicator.伸长;
            }else if (reachMaxLong)
            {
                skillState = skillIndicator.维持;
            }
        }
    }

    /// <summary>
    /// 伸长
    /// </summary>
    public void Long()
    {
        if(skillState == skillIndicator.伸长)
        {
            currSize += longSpeed;
            if (currSize >= maxLongX)
            {
                reachMaxLong = true;
            }
            sr.size = new Vector2(currSize, sr.size.y);
            lastPoint.position = beilv * direction.normalized * currSize / 10 + (Vector2)indicator.transform.position;
        }
    }

    /// <summary>
    /// 维持
    /// </summary>
    public void LongKeep()
    {
        if (skillState == skillIndicator.维持)
        {
            currSize = maxLongX;
            currkeeptime -= Time.deltaTime;
            if (currkeeptime <= 0)
            {
                isRush = true;
                SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[29]);
                currkeeptime = Keeptime;
                reachMaxLong = false;
                lastPoint.position = beilv * direction.normalized * currSize / 10 + (Vector2)indicator.transform.position;
                //skillState = skillIndicator.复原;
                canAttack = false;
            }
            else
            {
                lastPoint.position = beilv * direction.normalized * currSize / 10 + (Vector2)indicator.transform.position;
            }
            //sr.size = new Vector2(currSize, sr.size.y);
            //lastPoint.position = direction.normalized * currSize / 10 + (Vector2)indicator.transform.position;
        }
    }

    /// <summary>
    /// 攻击倒计时
    /// </summary>
    public void AttackTiming()
    {
        if (!canAttack)
        {
            currAttackCD -= Time.deltaTime;
            if(currAttackCD <= 0)
            {
                currAttackCD = attackCD;
                canAttack = true;
            }
        }
    }

    /// <summary>
    /// 冲刺
    /// </summary>
    public void Rush()
    {
        sr.color = new Color(255, 255, 255, 0);
        transform.position = Vector2.MoveTowards(transform.position, lastPoint.position, rushSpeed * Time.deltaTime);//移动
        if (Mathf.Abs(transform.position.x - lastPoint.position.x) + Mathf.Abs(transform.position.y - lastPoint.position.y) < 0.01)
        {
            isRush = false;
            skillState = skillIndicator.复原;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRush) return;
        Vector3 dir = transform.TransformVector(Vector3.right);
        dir.z = 0;
        CameraControl.instance.CameraInjure(transform.position - collision.transform.position);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<MoveObject>().GetHurt(damage, dir);
        }
            isRush = false;
            skillState = skillIndicator.复原;
    }
    public override void CollideWhenRush(string collisionTag)
    {
        isRush = false;
        skillState = skillIndicator.复原;
    }

    public override bool MouseBtnLeftDown(Vector2 targetPos)
    {
        return false;
    }
    public override void MouseBtnRightDown(Vector2 targetPos)
    {
        
    }
}
