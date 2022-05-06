using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Boss : MonoBehaviour
{
    public float HP = 1000f;
    protected float curHP = 1000f;
    public Slider HpSlider;
    public float speed = 2f;
    protected float speedScale = 1f;
    public float attackDis = 3f;
    protected Collider2D collider2D;
    [Header("图片")]
    public Transform GFX;
    public Animator[] animators;
    public Transform foot;

    public PlayerController player;
    public MoveObject targetMO;

    //寻路
    protected Seeker seeker;
    protected float nextWaypointDistance = 0.5f;
    protected Path path;
    protected bool reached = true;
    protected int currentWaypoint = 0;
    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        collider2D = GetComponent<Collider2D>();
        curHP = HP;
    }

    public Vector2 GetDirToPlayer()
    {
        if (targetMO)
        {
            return targetMO.transform.position - foot.position;
        }
        return Vector2.zero;
    }
    public void UpdatePath()
    {
        if (!seeker || reached) return;
        if (seeker.IsDone())
        {
            seeker.StartPath(foot.position, targetMO.transform.position, OnPathComplete);
        }
    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void GetHurt(float value)
    {
        curHP -= value;
        if (curHP <= 0)
        {
            Dead();
        }
        else
        {
            HpSlider.value = curHP / HP;
        }
    }
    protected virtual void Dead()
    {
        animators[0].SetTrigger("dead");
        HpSlider.gameObject.SetActive(false);
        collider2D.enabled = false;
        this.enabled = false;
    }
}
