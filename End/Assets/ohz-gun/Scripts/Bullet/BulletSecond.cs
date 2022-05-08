using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSecond : MonoBehaviour
{
    [Header("子弹基础属性")]
    public float speed;
    public float destoryTime;
    public float damage;

    private void Awake()
    {
        transform.SetParent(GameObject.Find("BulletManager").transform);
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
        DestoryItSelf();
    }

    /// <summary>
    /// 发射倒计时，销毁自身
    /// </summary>
    public void DestoryItSelf()
    {
        destoryTime -= Time.deltaTime;
        if (destoryTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Table"))
        {
            Destroy(gameObject);
        }
        Vector3 dir = transform.TransformVector(Vector3.right);
        dir.z = 0;

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<MoveObject>().GetHurt(damage, dir);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<MoveObject>().GetHurt(damage, dir);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Boom")
        {            
            collision.gameObject.GetComponent<ExplosiveBarrels>().CallBoomAnim();
            Destroy(gameObject);
        }
    }
}
