using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    public float speed = 10f;
    public float LifeTime = 10f;
    private bool col = false;
    [Header("子弹属性")]
    public float damage;

    public bool targetIsPlayer = true;
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
        Destroy(this.gameObject);
    }
    private void Update()
    {
        if (col) return;
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformPoint(Vector3.right), speed * Time.deltaTime,LayerMask.NameToLayer("Physics"));
        //if (hit)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    transform.Translate(Vector3.right * speed, Space.Self);
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (gameObject.tag == "Enemy")
            {
                Destroy(this.gameObject);
            }
        }
        Vector3 dir = (collision.transform.position - transform.position).normalized;
        dir.z = 0;
        if (collision.gameObject.tag == "Player" && targetIsPlayer)
        {
            col = true;
            GetComponent<Animator>().SetTrigger("destroy");
            
            collision.gameObject.GetComponent<Role>().GetHurt(10, RoleType.Physics,dir);
        }
        if (collision.gameObject.tag == "Enemy" && !targetIsPlayer)
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
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Table"))
        {
            GetComponent<Animator>().SetTrigger("destroy");
        }
        Vector3 dir = (collision.transform.position - transform.position).normalized;
        dir.z = 0;
        if (collision.gameObject.tag == "Player" && targetIsPlayer)
        {
            col = true;
            GetComponent<Animator>().SetTrigger("destroy");

            collision.gameObject.GetComponent<Role>().GetHurt(10, RoleType.Physics, dir);
        }
        if (collision.gameObject.tag == "Enemy" && !targetIsPlayer)
        {
            col = true;
            GetComponent<Animator>().SetTrigger("destroy");
            collision.gameObject.GetComponent<Role>().GetHurt(10, RoleType.Physics, dir);
        }
    }
}
