using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Boss : MonoBehaviour
{
    public float speed = 2f;
    protected float speedScale = 1f;
    public float attackDis = 3f;
    
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

    
}
