﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : Controller
{
    enum EnemyState
    {
        Ori,
        Attack,
        Run
    }
    private EnemyState state;

    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
    private Vector2 Target;
    private MoveObject Player;

    private bool Reach = false;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        state = EnemyState.Ori;
        SetMoveObject(GetComponent<MoveObject>());
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(moveObject.foot.position, Target, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Run)
        {
            if (Vector2.Distance(Player.transform.position, moveObject.foot.position) < moveObject.FearRadius)
            {
                Target = moveObject.foot.position + (Player.transform.position - moveObject.foot.position).normalized * -5;
                //恐惧表情
                Vector2 dir = Player.transform.position - moveObject.foot.position.normalized;
                MoveVelocity(dir, 1);
            }
        }
        else if (state == EnemyState.Attack)
        {
            if(Vector2.Distance(Player.transform.position, transform.position) < moveObject.attackRadius)
            {
                moveObject.MouseBtnLeft(Player.transform.position);
                Reach = true;
            }
            else
            {
                Reach = false;
            }
        }
    }
    float _t1 = 0f;
    private void FixedUpdate()
    {

        if (path == null || Reach)
        {
            MoveVelocity(Vector2.zero, 0f);
            return;
        }
        Vector2 direction = (path.vectorPath[currentWaypoint] - moveObject.foot.position).normalized;
        MoveVelocity(direction,1f);

        float distance = Vector2.Distance(moveObject.foot.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            if (path.vectorPath.Count != 1)
            {
                float dirForAnim = path.vectorPath[currentWaypoint].x - path.vectorPath[currentWaypoint - 1].x;
                TurnTowards(dirForAnim < 0);
            }
        }
    }
    public void SetPlayer(MoveObject moveObject)
    {
        Player = moveObject;
        if (Player.type == MoveObjectType.Dead)
        {
            state = EnemyState.Run;
        }
        else
        {
            state = EnemyState.Attack;
            Target = Player.transform.position;
            InvokeRepeating(nameof(UpdatePath), 0, 0.5f);
        }
    }
    public void NoFindPlayer()
    {
        Player = null;
        state = EnemyState.Ori;
        CancelInvoke(nameof(UpdatePath));
    }
}