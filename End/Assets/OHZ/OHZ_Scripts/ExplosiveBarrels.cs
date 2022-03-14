using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrels : MonoBehaviour
{
    public float radius = 5f;//爆炸范围
    public LayerMask targetLayer;
    public float boomForce = 10f;
    void Update()
    {
        //#对接#
        //替换成只要受到伤害，播放爆炸动画（下方函数均在爆炸动画中引用）
        //#对接#
        GetComponent<Animator>().Play("bomb");
    }

    public void CloseShadow()
    {
        transform.GetChild(0).gameObject.SetActive(false);
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
            //看看击退效果合不合适，加入受伤判定
            //#对接#
            Vector3 pos = transform.position - hit.transform.position;
            hit.GetComponent<Rigidbody2D>().AddForce(-pos.normalized * boomForce, ForceMode2D.Impulse);
        }
    }

    public void DestoryBarren()
    {
        Destroy(transform.parent.gameObject);
    }
}
