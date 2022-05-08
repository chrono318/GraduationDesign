using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public int bulletCount = 5;
    public GameObject bullet;
    public float attackCD = 8;
    float currAttackCD;
    bool isAttack = false;
    public float offsetY;
    float offsetX;

    Animator animator;

    private void Start()
    {
        currAttackCD = attackCD;
        animator = GetComponent<Animator>();
        animator.Play("idle");
    }

    private void Update()
    {
        if (!isAttack)
        {
            animator.Play("attack");
        }
        else
        {
            CountDown();
        }
    }

    /// <summary>
    /// 生成子弹
    /// </summary>
    public void CreatBullet()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            int angle = Random.Range(0, 360);
            offsetX = Random.Range(-1, 1);
            GameObject obj1 = Instantiate(bullet, new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), Quaternion.Euler(0, 0, angle), transform);
        }
        isAttack = true;
    }

    /// <summary>
    /// 攻击冷却
    /// </summary>
    public void CountDown()
    {
        currAttackCD -= Time.deltaTime;
        if(currAttackCD <= 0)
        {
            currAttackCD = attackCD;
            isAttack = false;
        }
    }
}
