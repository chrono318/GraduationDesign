using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Award : MonoBehaviour
{
    GameObject player;
    Transform top, down;

    /// <summary>
    /// 是否可以显示提示UI,用这个控制UI是否显示
    /// </summary>
    private bool m_canSetActiveUI;
    public bool canSetActiveUI
    {
        get
        {
            return m_canSetActiveUI;
        }
        private set
        {
            m_canSetActiveUI = value;
        }
    }

    /// <summary>
    /// 玩家是否可以拾取
    /// </summary>
    private bool m_canPickUp;
    public bool canPickUp
    {
        get
        {
            return m_canPickUp;
        }
        private set
        {
            m_canPickUp = value;
        }
    }

    /// <summary>
    /// 该物体是否是生成的
    /// </summary>
    private bool m_isCreat = false;
    public bool isCreat
    {
        get
        {
            return m_isCreat;
        }
        set
        {
            m_isCreat = value;
        }
    }

    /// <summary>
    /// 上升与下落速度
    /// </summary>
    private float m_speed = 3f;
    public float speed
    {
        get
        {
            return m_speed;
        }
        set
        {
            m_speed = value;
        }
    }

    /// <summary>
    /// 是否到达最高点
    /// </summary>
    private bool m_reachTop = false;
    public bool reachTop
    {
        get
        {
            return m_reachTop;
        }
        set
        {
            m_reachTop = value;
        }
    }

    /// <summary>
    /// 是否到达最低点
    /// </summary>
    private bool m_reachDown = false;
    public bool reachDown
    {
        get
        {
            return m_reachDown;
        }
        set
        {
            m_reachDown = value;
        }
    }

    /// <summary>
    /// 暂留倒计时设置
    /// </summary>
    [SerializeField]
    private float m_countDownTime;
    public float countDownTime
    {
        get
        {
            return m_countDownTime;
        }
        set
        {
            m_countDownTime = value;
        }
    }

    /// <summary>
    /// 暂留倒计时
    /// </summary>
    private float m_currCountDownTime;
    public float currCountDownTime
    {
        get
        {
            return m_currCountDownTime;
        }
        set
        {
            m_currCountDownTime = value;
        }
    }

    /// <summary>
    /// 是否结束暂留
    /// </summary>
    private bool m_isOver;
    public bool isOver
    {
        get
        {
            return m_isOver;
        }
        set
        {
            m_isOver = value;
        }
    }

    /// <summary>
    /// 检测玩家是否按下交互键
    /// </summary>
    private bool m_isPressInterationButton;
    public bool isPressInterationButton
    {
        get
        {
            return m_isPressInterationButton;
        }
        private set
        {
            m_isPressInterationButton = value;
        }
    }

    /// <summary>
    /// 到最高点已跟随时间
    /// </summary>
    private float m_topFollowedTime = 0f;
    public float topFollowedTime
    {
        get
        {
            return m_topFollowedTime;
        }
        private set
        {
            m_topFollowedTime = value;
        }
    }

    /// <summary>
    /// 到最高点跟随总时间
    /// </summary>
    [SerializeField]
    private float m_topFollowSumTime = 3f;
    public float topFollowSumTime
    {
        get
        {
            return m_topFollowSumTime;
        }
        set
        {
            m_topFollowSumTime = value;
        }
    }

    /// <summary>
    /// 到最低点已跟随时间
    /// </summary>
    private float m_downFollowedTime = 0f;
    public float downFollowedTime
    {
        get
        {
            return m_topFollowedTime;
        }
        private set
        {
            m_topFollowedTime = value;
        }
    }

    /// <summary>
    /// 到最低点跟随总时间
    /// </summary>
    [SerializeField]
    private float m_downFollowSumTime = 3f;
    public float downFollowSumTime
    {
        get
        {
            return m_topFollowSumTime;
        }
        set
        {
            m_topFollowSumTime = value;
        }
    }

    private void Start()
    {
        if (transform.parent) //查看此物体是动态生成还是摆放的
        {
            if(transform.parent.gameObject.GetComponent<InteractionObject>())
            {
                isCreat = true;
            }
            else
            {
                isCreat = false;
            }
        }
        else
        {
            isCreat = false;
        }

        top = transform.GetChild(0).gameObject.transform;
        down = transform.GetChild(1).gameObject.transform;
        transform.DetachChildren();

        currCountDownTime = countDownTime;
    }

    private void Update()
    {
        CreatEffect();
        CheckPressInteraction();
        PickUpAward();

    }

    /// <summary>
    /// 出生效果
    /// </summary>
    public void CreatEffect()
    {
        if (isCreat)
        {
            if(!reachTop && !reachDown && !isOver)//上升
            {
                //transform.position = Vector2.MoveTowards(transform.position, top.position, speed * Time.deltaTime);
                topFollowedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, top.position, topFollowedTime / topFollowSumTime);
                if (Vector2.Distance(transform.position,top.position) < 0.1f)
                {
                    reachTop = true;
                    speed = 0;
                }
            }
            else if (reachTop && !reachDown && !isOver)//暂留
            {
                if (!isOver)
                {
                    currCountDownTime -= Time.deltaTime;
                    if(currCountDownTime <= 0)
                    {
                        currCountDownTime = countDownTime;
                        isOver = true;
                        speed = 8;
                    }
                }
            }
            else if (reachTop && !reachDown && isOver)//下落
            {
                //transform.position = Vector2.MoveTowards(transform.position, down.position, speed * Time.deltaTime);
                downFollowedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, down.position, downFollowedTime / downFollowSumTime);
                if (Vector2.Distance(transform.position, down.position) < 0.1f)
                {
                    reachDown = true;
                }
            }
            else
            {
                canPickUp = true;
            }
        }
    }

    /// <summary>
    /// 查看玩家是否按下交互键
    /// </summary>
    public void CheckPressInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPressInterationButton = true;
        }
        else if(Input.GetKeyUp(KeyCode.E))
        {
            isPressInterationButton = false;
        }
    }

    /// <summary>
    /// 拾取奖励
    /// </summary>
    public void PickUpAward()
    {
        if(isPressInterationButton && canSetActiveUI)
        {
            print("已拾取");
            //
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canPickUp)
        {
            canSetActiveUI = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSetActiveUI = false;
        }
    }
}
