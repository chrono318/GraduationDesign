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
        //替换成只要受到伤害s
        //#对接#
        if (Input.GetMouseButtonDown(0))
        {
            Boom();
        }
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
}
