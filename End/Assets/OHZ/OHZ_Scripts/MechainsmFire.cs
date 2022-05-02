using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechainsmFire : MonoBehaviour
{
    public GameObject bullet;
    public bool isFire;
    protected Pool bulletPool;
    public enum fireDir
    {
        正向 = 0,
        左向 = 1,
        右向 = 2
    }

    public fireDir dir;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isFire)
        {
            animator.Play("pop-out");
        }
    }
    public void CreatBullet()
    {
        GameObject obj1 = Instantiate(bullet, transform.GetChild(0).position, Quaternion.identity, transform);
    }

    public void CloseFire()
    {
        animator.Play("push-in");
        isFire = false;
    }

}
