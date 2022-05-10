using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seagulls : MonoBehaviour
{
    public ParticleSystem particleSystem;
    //#对接#
    //添加敌人状态下，受伤闪白表现及受伤函数（含参数）
    //添加玩家状态下，受伤效果
    //#对接#

    //#对接#
    //添加敌人状态与玩家状态下移动脚本（含参数）
    //#对接#

    //#对接#
    //添加玩家状态下技能效果（脚本类似于弹球，只不过多了加速及加速时间）
    //加速拖尾？我去看看以前的项目
    //#对接#

    Rigidbody2D rb;
    Animator anim;

    //#对接#被玩家附身一瞬间播放reback动画#对接#
    //#对接#敌人、玩家受伤播放受伤动画，有受伤函数，在哪里写受伤和动画#对接#
    //#对接#敌人、玩家死亡播放死亡动画#对接#
    //#对接#敌人死亡播放完死亡动画后，播放stock动画#对接#

    /// <summary>
    /// 是否是玩家操控（如果你不需要这个参数，删了就好）
    /// </summary>
    public bool isPlayer;
    /// <summary>
    /// 玩家状态下参数
    /// </summary>
    #region
    int attackPressNum = 0;
    bool playerReady;
    bool haveBoom;//是否爆炸过
    public float currAttackBoomCD;
    public float attackBoomCD = 5f;

    /// <summary>
    /// 玩家状态下，爆炸时间提示
    /// </summary>
    public GameObject clock;

    /// <summary>
    /// 眩晕标记
    /// </summary>
    public GameObject dizzy;

    /// <summary>
    /// 敌人状态下，爆炸预警提示
    /// </summary>
    public GameObject beScare;
    #endregion

    /// <summary>
    /// 敌人状态下参数
    /// </summary>
    #region
    GameObject player;
    public Vector3 targetPoint;
    public float speed = 10;

    bool isReach;//是否到达目标点

    /// <summary>
    /// 休息时长
    /// </summary>
    float currRestCD;
    public float restCD = 1.5f;

    /// <summary>
    /// 爆炸预备倒计时
    /// </summary>
    float currReadyBoom;
    public float readyBoom = 2f;
    bool switchBoomOpen;
    public float boomForce = 10f;
    public float boomDistance = 5f;//直接引爆爆炸距离（当海鸥准备爆炸且玩家位于这个距离内是直接引爆）

    public float radius = 5f;
    public LayerMask targetLayer;

    /// <summary>
    /// 眩晕有关参数
    /// </summary>
    float currVertigoTime;
    public float vertigoTime = 3f;
    public float impulseForce = 20f;//撞击被弹开力度(配合rb的阻尼一起使用)
    Vector3 vertigoMoveNor;//记录海鸥与墙体之间的向量
    #endregion

    /// <summary>
    /// 敌人的状态
    /// </summary>
    #region
    public enum SeagullState
    {
        默认 = 0,
        移动,
        冲刺,
        爆炸,
        休息,
        眩晕
    }

    public SeagullState state;
    public SeagullState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (value)
                {
                    case SeagullState.默认:
                        anim.SetInteger("animState", 0);
                        break;
                    case SeagullState.移动:
                        anim.SetInteger("animState", 0);
                        break;
                    case SeagullState.冲刺:
                        anim.SetInteger("animState", 1);
                        targetPoint = player.transform.position;
                        break;
                    case SeagullState.爆炸:
                        anim.SetInteger("animState", 2);
                        haveBoom = true;
                        clock.SetActive(false);
                        currAttackBoomCD = attackBoomCD;
                        beScare.SetActive(false);
                        break;
                    case SeagullState.休息:
                        anim.SetInteger("animState", 0);
                        break;
                    case SeagullState.眩晕:
                        Injure();
                        rb.velocity = Vector3.zero;
                        dizzy.SetActive(true);
                        rb.AddForce(vertigoMoveNor.normalized * impulseForce, ForceMode2D.Impulse);
                        break;
                    default:
                        return;
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// 共用函数
    /// </summary>
    #region
    /// <summary>
    /// 时间参数初始化
    /// </summary>
    public void InitTime()
    {
        currRestCD = restCD;
        currReadyBoom = readyBoom;
        currAttackBoomCD = attackBoomCD;
        currVertigoTime = vertigoTime;
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    public void Boom()
    {
        Collider2D[] arround = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (var hit in arround)
        {
            //#对接#
            //看看击退效果合不合适
            //#对接#
            Vector3 pos = transform.position - hit.transform.position;
            hit.GetComponent<Rigidbody2D>().AddForce(-pos.normalized * boomForce, ForceMode2D.Impulse);
        }
    }
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = transform.parent.GetComponent<Rigidbody2D>();
        InitTime();
        State = SeagullState.默认;
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (isPlayer)
        {
            CheckAttackPress();
            PlayerReadyBoom();
        }
        else
        {
            ChangeState();
            Rush();
            Rest();
            ReadyBoom();
            Vertigo();
        }
    }

    /// <summary>
    /// 玩家状态下函数
    /// </summary>
    #region
    /// <summary>
    /// 攻击按键检测
    /// </summary>
    public void CheckAttackPress()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attackPressNum += 1;
        }
    }

    /// <summary>
    /// 玩家要炸了
    /// </summary>
    public void PlayerReadyBoom()
    {

        if (attackPressNum == 1 && !haveBoom)
        {
            playerReady = true;
            //#对接#
            //在此处调用下Tips，tips内容再次点击立即爆炸
            //#对接#
            clock.SetActive(true);
            Clock();
        }
        else if (attackPressNum > 1 && !haveBoom)
        {
            State = SeagullState.爆炸;
        }
        if (playerReady)
        {
            currAttackBoomCD -= Time.deltaTime;
            if (currAttackBoomCD <= 0)
            {
                State = SeagullState.爆炸;

            }
        }
    }

    /// <summary>
    /// 爆炸时间显示
    /// </summary>
    public void Clock()
    {
        //#对接#
        //把钟表移动至海鸥头顶，并跟着海鸥一起动（俺不会UI移动）
        //#对接#
        //蓝色填充移动
        clock.transform.Find("CD").GetComponent<Image>().fillAmount -= -1.0f / attackBoomCD * Time.deltaTime;
        //指针旋转
        clock.transform.Find("Pointer").gameObject.transform.rotation = Quaternion.Euler(0, 0, 90 + clock.transform.Find("CD").GetComponent<Image>().fillAmount * -360);
    }
    #endregion

    /// <summary>
    /// 敌人状态下函数
    /// </summary>
    #region
    /// <summary>
    /// 切换敌人状态
    /// </summary>
    public void ChangeState()
    {
        //# 对接#
        //如果玩家与该名敌人位于同一个房间
        //则海鸥向玩家方向移动 
        //#对接#

        if (transform.Find("CheckArea").GetComponent<CheckArea>().findPlayer && State == SeagullState.默认)
        {
            State = SeagullState.冲刺;
        }
        if (isReach)
        {
            State = SeagullState.休息;
        }
    }

    /// <summary>
    /// 冲刺
    /// </summary>
    public void Rush()//冲刺不用进行A*寻路
    {
        //#冲刺状态下加残影或特效#对接#
        if (State == SeagullState.冲刺)
        {
            transform.parent.gameObject.transform.position = Vector2.MoveTowards(transform.parent.gameObject.transform.position, targetPoint, speed * Time.deltaTime);//移动
            if (Mathf.Abs(transform.parent.gameObject.transform.position.x - targetPoint.x) + Mathf.Abs(transform.parent.gameObject.transform.position.y - targetPoint.y) < 0.01)
            {
                isReach = true;
                if (isReach && switchBoomOpen)//如果海鸥到达指定位置且已经准备好爆炸
                {
                    State = SeagullState.爆炸;
                }
            }
            if (Mathf.Abs(transform.parent.gameObject.transform.position.x - player.transform.position.x) + Mathf.Abs(transform.parent.gameObject.transform.position.y - player.transform.position.y) < boomDistance && switchBoomOpen) //如果海鸥与玩家距离过近且已经准备好爆炸
            {
                State = SeagullState.爆炸;
            }
        }
    }

    /// <summary>
    /// 攻击完累了，摸会鱼
    /// </summary>
    public void Rest()
    {
        if (State == SeagullState.休息)
        {
            currRestCD -= Time.deltaTime;
            if (currRestCD <= 0)
            {
                State = SeagullState.默认;
                isReach = false;
                currRestCD = restCD;
            }
        }
    }

    /// <summary>
    /// 准备爆炸
    /// </summary>
    public void ReadyBoom()
    {
        if (transform.Find("Waring").GetComponent<BoomWaring>().isReadyBoom && State == SeagullState.冲刺)
        {
            switchBoomOpen = true;
            anim.SetInteger("animState", 4);
            beScare.SetActive(true);
        }
        if (switchBoomOpen)
        {
            currReadyBoom -= Time.deltaTime;
            if (currReadyBoom <= 0)
            {
                currReadyBoom = readyBoom;
                State = SeagullState.爆炸;
            }
        }
    }

    /// <summary>
    /// 撞晕了
    /// </summary>
    public void Vertigo()
    {
        if (State == SeagullState.眩晕)
        {
            currVertigoTime -= Time.deltaTime;
            if (currVertigoTime <= 0)
            {
                State = SeagullState.默认;
                isReach = false;
                currVertigoTime = vertigoTime;
                dizzy.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 敌人状态爆炸死亡
    /// </summary>
    public void EnemyDeadBoom()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public void Injure()
    {
        anim.SetInteger("animState", 3);
    }

    /// <summary>
    /// 从受伤状态恢复
    /// </summary>
    public void Restore()
    {
        if (anim == null)
            print("no anim");
        anim.SetInteger("animState", 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        return;
        //#对接#
        //将条件从Wall改为如果碰到高大的障碍物或墙体//
        //#对接#
        if (collision.gameObject.CompareTag("Wall") && State == SeagullState.冲刺)
        {
            //State = SeagullState.爆炸;
            if (switchBoomOpen)
            {
                State = SeagullState.爆炸;
            }
            else
            {
                Vector2 Pos = transform.position - collision.gameObject.transform.position;
                vertigoMoveNor = new Vector3(Pos.x, Pos.y, 0);
                State = SeagullState.眩晕;
            }
        }
    }
    #endregion
    public void ShowParticle()
    {
        particleSystem.Play();
    }
}
