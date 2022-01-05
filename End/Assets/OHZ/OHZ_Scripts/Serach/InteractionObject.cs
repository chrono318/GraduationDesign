using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    GameObject player;

    /// <summary>
    /// 是否可以进行交互
    /// </summary>
    [SerializeField]
    private bool m_canInteraction = false;
    public bool canInetraction
    {
        get
        {
            return m_canInteraction;
        }
        set
        {
            m_canInteraction = value;
        }
    }

    /// <summary>
    /// 可交互次数
    /// </summary>
    [SerializeField]
    private int m_canInteractionNum = 1;
    public int canInteractionNum
    {
        get
        {
            return m_canInteractionNum;
        }
        set
        {
            m_canInteractionNum = value;
        }
    }

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
    /// 是否与玩家发生交互
    /// </summary>
    private bool m_isCollsion;
    public bool isCollsion
    {
        get
        {
            return m_isCollsion;
        }
        private set
        {
            m_isCollsion = value;
        }
    }

    private int[] weight = new int[4];

    //没搜刮到拾取物、搜刮到三等拾取物、搜刮到二等拾取物、搜刮到一等拾取物
    public int noAwardPro = 50;
    public int thirdAwardPro = 30;
    public int secondAwardPro = 15;
    public int fristAwardPro = 5;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weight[0] = noAwardPro;
        weight[1] = thirdAwardPro;
        weight[2] = secondAwardPro;
        weight[3] = fristAwardPro;
    }

    void Update()
    {
        CheckCanInteraction();
    }

    /// <summary>
    /// 查看玩家是否按下交互键
    /// </summary>
    public void CheckPressInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isPressInterationButton = true;
            canInteractionNum -= 1;
            canInetraction = false;
            Search();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            isPressInterationButton = false;
        }
    }

    /// <summary>
    /// 查看是否可以交互
    /// </summary>
    public void CheckCanInteraction()
    {
        if(isCollsion && canInteractionNum > 0)
        {
            canInetraction = true;
            canSetActiveUI = true;
            CheckPressInteraction();
        }
        else
        {
            canInetraction = false;
            canSetActiveUI = false;
        }
    }

    /// <summary>
    /// 生成搜查奖励
    /// </summary>
    public void Search()
    {
        int probability = GetRandPersonalityType(weight,100);
        int getAward;
        switch (probability)
        {
            case 0:
                print("这里空空如也");
                break;
            case 1:
                print("搜刮到三等拾取物");
                getAward = Random.Range(0, AwardManager.instance.awardLevelThree.Count);
                Instantiate(AwardManager.instance.awardLevelThree[getAward], transform.position, Quaternion.identity, this.transform);
                break;
            case 2:
                print("搜刮到二等拾取物");
                getAward = Random.Range(0, AwardManager.instance.awardLevelTwo.Count);
                Instantiate(AwardManager.instance.awardLevelTwo[getAward], transform.position, Quaternion.identity, this.transform);
                break;
            case 3:
                print("搜刮到一等拾取物");
                getAward = Random.Range(0, AwardManager.instance.awardLevelOne.Count);
                Instantiate(AwardManager.instance.awardLevelOne[getAward], transform.position, Quaternion.identity, this.transform);
                break;
            default:
                return;
        }
    }

    private int GetRandPersonalityType(int[] array, int _total)
    {
        int rand = Random.Range(1, _total + 1);
        int tmp = 0;

        for (int i = 0; i < array.Length; i++)
        {
            tmp += array[i];
            if (rand < tmp)
            {
                return i;
            }
        }
        return 0;
    }

    //视情况若需更大的交互范围与提示范围，则挂载在控制交互范围的物体上
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollsion = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollsion = false;
        }
    }
}
