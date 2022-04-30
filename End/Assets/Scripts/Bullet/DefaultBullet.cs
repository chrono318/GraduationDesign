using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : Bullet
{
    public float LifeTime = 10f;
    [Header("子弹属性")]
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(Bullet));
    }
    private IEnumerator Bullet()
    {
        yield return new WaitForSeconds(LifeTime);
        GetComponent<Animator>().SetTrigger("destroy");
    }
    public void AnimEnd()
    {
        //gameObject.SetActive(false);
        DestroyBulletSelf();
    }
    private void FixedUpdate()
    {
        if (col) return;
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        //transform.position += (Vector3)dir * speed * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (!isPlayer)
            {
                DestroyBulletSelf();
            }
        }
        Vector3 dir = (collision.transform.position - transform.position).normalized;
        dir.z = 0;
        if (collision.gameObject.tag == "Player" && !isPlayer)
        {
            col = true;
            GetComponent<Animator>().SetTrigger("destroy");
            
            collision.gameObject.GetComponent<Role>().GetHurt(10, RoleType.Physics,dir);
        }
        if (collision.gameObject.tag == "Enemy" && isPlayer)
        {
            col = true;
            GetComponent<Animator>().SetTrigger("destroy");
            collision.gameObject.GetComponent<Role>().GetHurt(10, RoleType.Physics,dir);
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position+transform.TransformPoint(Vector3.right) * speed * Time.deltaTime);
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (gameObject.tag == "Enemy")
            {
                DestroyBulletSelf();
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Table"))
        {
            GetComponent<Animator>().SetTrigger("destroy");
        }
        Vector3 dir = transform.TransformVector(Vector3.right);
        dir.z = 0;
        if (collision.gameObject.tag == "Player" && !isPlayer)
        {
            col = true;
            GetComponent<Animator>().SetTrigger("destroy");

            collision.gameObject.GetComponent<MoveObject>().GetHurt(10, dir);
        }
        if (collision.gameObject.tag == "Enemy" && isPlayer)
        {
            col = true;
            GetComponent<Animator>().SetTrigger("destroy");
            collision.gameObject.GetComponent<MoveObject>().GetHurt(10, dir);
        }
        if (collision.gameObject.tag == "Boom")
        {
            collision.gameObject.GetComponent<ExplosiveBarrels>().CallBoomAnim();
        }
    }
}
