using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagulls : MonoBehaviour
{
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
    float currAttackBoomCD;
    public float attackBoomCD = 5f;
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
                        break;
                    case SeagullState.移动:
                        break;
                    case SeagullState.冲刺:
                        targetPoint = player.transform.position;
                        break;
                    case SeagullState.爆炸:
                        Boom();
                        break;
                    case SeagullState.休息:
                        break;
                    case SeagullState.眩晕:
                        rb.velocity = Vector3.zero;
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
        //死亡
        Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody2D>();
        InitTime();
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

        if (attackPressNum == 1)
        {
            playerReady = true;
            //#对接#
            //在此处调用下Tips
            //#对接#
        }
        else if (attackPressNum > 1)
        {
            Boom();
        }
        if (playerReady)
        {
            currAttackBoomCD -= Time.deltaTime;
            if (currAttackBoomCD <= 0)
            {
                currAttackBoomCD = attackBoomCD;
                Boom();
            }
        }
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

        if (transform.GetChild(0).GetComponent<CheckArea>().findPlayer && State == SeagullState.默认)
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
        if (State == SeagullState.冲刺)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);//移动
            if (Mathf.Abs(transform.position.x - targetPoint.x) + Mathf.Abs(transform.position.y - targetPoint.y) < 0.01)
            {
                isReach = true;
                if (isReach && switchBoomOpen)//如果海鸥到达指定位置且已经准备好爆炸
                {
                    State = SeagullState.爆炸;
                }
            }
            if (Mathf.Abs(transform.position.x - player.transform.position.x) + Mathf.Abs(transform.position.y - player.transform.position.y) < boomDistance && switchBoomOpen) //如果海鸥与玩家距离过近且已经准备好爆炸
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
        if (transform.GetChild(1).GetComponent<BoomWaring>().isReadyBoom && State == SeagullState.冲刺)
        {
            switchBoomOpen = true;
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
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
}
