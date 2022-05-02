using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMechanism : MonoBehaviour
{
    [Header("子弹基础属性")]
    public float speed;
    public float destoryTime;
    GameObject firePoint;
    Vector3 currFireDir;
    GameObject fire;
    Animator animator;
    public float damage;

    private void Awake()
    {
        fire = transform.parent.gameObject;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.SetParent(GameObject.Find("BulletManager").transform);
        if (fire.GetComponent<MechainsmFire>().dir == MechainsmFire.fireDir.正向)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            currFireDir = Vector3.right;
        }
        else if (fire.GetComponent<MechainsmFire>().dir == MechainsmFire.fireDir.右向)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            currFireDir = Vector3.right;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            currFireDir = Vector3.left;
        }
    }

    void Update()
    {
        transform.Translate(currFireDir * Time.deltaTime * speed, Space.Self);
        TimeOut();
    }

    /// <summary>
    /// 发射倒计时，销毁自身
    /// </summary>
    public void TimeOut()
    {
        destoryTime -= Time.deltaTime;
        if (destoryTime <= 0)
        {
            DestoryItSelf();
        }
    }

    public void DestoryItSelf()
    {
        animator.SetTrigger("destroy");
    }

    public void AnimEnd()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
             DestoryItSelf();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Table"))
        {
            DestoryItSelf();
        }
        Vector3 dir = transform.TransformVector(Vector3.right);
        dir.z = 0;

        if (collision.gameObject.tag == "Player")
        {
            DestoryItSelf();
            collision.gameObject.GetComponent<MoveObject>().GetHurt(damage, dir);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            DestoryItSelf();
            collision.gameObject.GetComponent<MoveObject>().GetHurt(damage, dir);
        }

        if (collision.gameObject.tag == "Boom")
        {
            DestoryItSelf();
            collision.gameObject.GetComponent<ExplosiveBarrels>().CallBoomAnim();
        }
    }

}
