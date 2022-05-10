using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    GameObject player;
    //public GameObject tentacle;
    public GameObject indication;

    public GameObject LifeFire;
    public int Life = 3;
    int currLife;

    [Header("攻击参数")]
    public float attackRadius = 20f;
    public float damage = 15f;
    public float attackCD = 8f;
    float currAttackCD;
    public bool canAttack = true;
    bool catchPlayer;

    //目标获取及攻击提示
    Vector2 targetPos;
    public float indicationTime;
    float currIndicationTime;
    public bool overIndicaction = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currAttackCD = attackCD;
        currIndicationTime = indicationTime;
        currLife = Life;
    }

    private void Update()
    {
        if (currLife > 0)
        {
            if (canAttack)
            {
                CatchPlayer();
            }
            else
            {
                AttackCountDown();
            }
        }
        else
        {
            LifeFire.GetComponent<ParticleSystem>().Stop();
            for (int i = 0; i < LifeFire.transform.childCount; i++)
            {
                LifeFire.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    /// <summary>
    /// 获取玩家位置
    /// </summary>
    public void CatchPlayer()
    {
        if (!catchPlayer)//捕捉玩家位置并开启技能指示
        {
            if(Vector2.Distance(transform.position,player.transform.position) < attackRadius)
            {
                targetPos = player.transform.position;
                catchPlayer = true;
                GameObject currIndication = Instantiate(indication, targetPos, Quaternion.identity, transform);
            }
        }
        
        //if(catchPlayer && overIndicaction)
        //{
        //    Attack();
        //}
    }

    /// <summary>
    /// 攻击
    /// </summary>
    //public void Attack()
    //{
    //    GameObject obj1 = Instantiate(tentacle, targetPos, Quaternion.identity, transform);
    //    canAttack = false;
    //}

    /// <summary>
    /// 攻击冷却
    /// </summary>
    public void AttackCountDown()
    {
        currAttackCD -= Time.deltaTime;
        if(currAttackCD <= 0)
        {
            currAttackCD = attackCD;
            canAttack = true;
            catchPlayer = false;
            overIndicaction = false;
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public void GetHurt(int damage)
    {
        currLife -= damage;
    }
}
