using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoleType
{
    Physics,
    San
}
[RequireComponent(typeof(Collider2D))]
public class Attack_Item : MonoBehaviour
{
    public LayerMask layerMask;
    public RoleType attackType;
    public float value;
    public float force = 1f;
    public Animator animator;
    public float AnimaPauseDuration = 0.1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        if ((layerMask.value & (int)Mathf.Pow(2,layer)) == (int)Mathf.Pow(2, layer))
        {
            OnHit(collision.gameObject);
            StartCoroutine(nameof(AnimaPause));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;
        if ((layerMask.value & (int)Mathf.Pow(2, layer)) == (int)Mathf.Pow(2, layer))
        {
            OnHit(collision.gameObject);
            if (animator)
            {
                //帧冻结不行
                //StartCoroutine(nameof(AnimaPause));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    public virtual void OnHit(GameObject col)
    {
        if(col.TryGetComponent<Role>(out Role role))
        {
            Vector3 dir = (col.transform.position - transform.position).normalized;
            dir.z = 0;
            role.GetHurt(value, attackType,dir*force);
        }
    }
    public virtual void Destroy()
    {

    }
    IEnumerator AnimaPause()
    {
        //if (animator)
        //{
        //    animator.speed = 0f;
        //    yield return new WaitForSeconds(AnimaPauseDuration);
        //    animator.speed = 1f;
        //}
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(AnimaPauseDuration);
        Time.timeScale = 1f;
    }
}
