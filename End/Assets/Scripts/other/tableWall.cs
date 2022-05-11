using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableWall : MonoBehaviour
{
    public Animator animator;
    int hp = 5;
    public void BulletAttack()
    {
        hp--;
        if (hp == 0)
        {
            Destroy(transform.parent.gameObject);
        }
        else if (hp == 3)
        {
            animator.Play("good - broken");
        }
        else if (hp == 2)
        {
            animator.Play("broken - bad");
        }
        else if (hp == 1)
        {
            animator.Play("idle-bad");
        }
    }
}
