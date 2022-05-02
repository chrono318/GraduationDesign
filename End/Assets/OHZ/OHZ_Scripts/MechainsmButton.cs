using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechainsmButton : MonoBehaviour
{
    public float restoreCD;
    float currRestoreCD;
    bool isTouch;
    bool canStart = true;
    public Transform[] fire;
    Animator animator;
    void Start()
    {
        currRestoreCD = restoreCD;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartMechainsm();
        ResetMechainsm();
    }

    /// <summary>
    /// 启用机关
    /// </summary>
    public void StartMechainsm()
    {
        if(canStart && isTouch)
        {
            //启动
            canStart = false;
            for (int i = 0; i < fire.Length; i++)
            {
                fire[i].GetComponent<MechainsmFire>().isFire = true;
            }
        }
    }

    /// <summary>
    /// 重置机关
    /// </summary>
    public void ResetMechainsm()
    {
        //如果机关进入无法启动状态且玩家没有踩下按钮
        //按钮恢复动画
        if(!canStart && !isTouch)
        {
            currRestoreCD -= Time.deltaTime;
            if(currRestoreCD <= 0)
            {
                currRestoreCD = restoreCD;
                canStart = true;
                GetComponent<BoxCollider2D>().enabled = true;
                animator.Play("pop");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouch = true;
            animator.Play("push");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouch = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
