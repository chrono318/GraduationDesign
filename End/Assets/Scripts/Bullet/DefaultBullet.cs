using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : Bullet
{
    private void OnEnable()
    {
        StartCoroutine(nameof(Bullet));
    }
    private IEnumerator Bullet()
    {
        yield return new WaitForSeconds(LifeTime);
        animator.SetTrigger("destroy");
    }
    public void AnimEnd()
    {
        //gameObject.SetActive(false);
        pool.ReturnGoToPool(this.gameObject);
    }

    void StartDestoryAnim()
    {
        col = true;
        animator.SetTrigger("destroy");
    }
    private void FixedUpdate()
    {
        if (col) return;
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        //transform.position += (Vector3)dir * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (col) return;
        if (collision.gameObject.tag == "Wall")
        {
            if (gameObject.tag == "Enemy")
            {
                DestroyBulletSelf();
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Table"))
        {
            animator.SetTrigger("destroy");
        }
        Vector3 dir = transform.TransformVector(Vector3.right);
        dir.z = 0;
        if (collision.gameObject.tag == "Player" && !isPlayer)
        {
            StartDestoryAnim();

            collision.gameObject.GetComponent<MoveObject>().GetHurt(damage, dir);
        }
        else if (collision.gameObject.tag == "Enemy" && isPlayer)
        {
            StartDestoryAnim();
            collision.gameObject.GetComponent<MoveObject>().GetHurt(damage, dir);
        }
        else if (collision.gameObject.tag == "Boom")
        {
            AnimEnd();
            collision.gameObject.GetComponent<ExplosiveBarrels>().CallBoomAnim();
        }
        else if (collision.gameObject.tag == "Boss" && isPlayer)
        {
            StartDestoryAnim();
            collision.gameObject.GetComponent<Boss>().GetHurt(damage);
        }
        else if(collision.gameObject.tag == "shenxiang")
        {
            StartDestoryAnim();
            collision.gameObject.GetComponent<Tentacle>().GetHurt(damage);
        }
    }
}
