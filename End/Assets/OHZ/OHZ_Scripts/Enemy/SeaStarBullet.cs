using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaStarBullet : MonoBehaviour
{
    [Header("子弹基础属性")]
    public float speed;
    public float destoryTime;

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
        DestroyItSelf();
    }

    /// <summary>
    /// 发射倒计时，销毁自身
    /// </summary>
    public void DestroyItSelf()
    {
        destoryTime -= Time.deltaTime;
        if (destoryTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))//碰到墙体或敌人销毁
        {
            Destroy(gameObject);
        }
    }
}
