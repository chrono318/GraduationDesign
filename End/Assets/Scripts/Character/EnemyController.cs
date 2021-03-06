using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : Controller
{
    enum EnemyState
    {
        Ori,
        toPlayer,
        Run,
        Attack
    }
    private EnemyState state;

    public float nextWaypointDistance = 0.5f;
    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
    private Vector2 Target;
    private MoveObject Player;

    private bool reach = false;
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

        if (state == EnemyState.Run && moveObject._State == MoveObject.State.Normal)
        {
            if (Vector2.Distance(Player.transform.position, moveObject.foot.position) < moveObject.FearRadius)
            {
                //Target = moveObject.foot.position + (Player.transform.position - moveObject.foot.position).normalized * -5;
                moveObject.fearTex.SetActive(true);
                //恐惧表情
                Vector2 dir = -Player.transform.position + moveObject.foot.position;
                MoveVelocity(dir.normalized, 0.5f);
                TurnTowards(dir.x < 0);
            }
            else
            {
                moveObject.fearTex.SetActive(false);
                MoveVelocity(Vector2.zero, 0f);
            }
        }
        else if (state == EnemyState.toPlayer)
        {
            moveObject.fearTex.SetActive(false);

            if (Vector2.Distance(Player.foot.position, transform.position) < moveObject.attackRadius)
            {
                moveObject.MouseBtnLeftDown(Player.transform.position);
                //state = EnemyState.Attack;
                reach = true;
            }
            else
            {
                reach = false;
                Target = Player.foot.position;
            }
        }
        else
        {
            moveObject.fearTex.SetActive(false);
        }
        if (weaponDir)
        {
            if(weaponDir._update && Player != null)
            {
                weaponDir.target = Player.transform.position;
            }
        }
    }
    private void FixedUpdate()
    {

        if (path == null || reach)
        {
            //MoveVelocity(Vector2.zero, 0f);
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count) return;
        Vector2 direction = (path.vectorPath[currentWaypoint] - moveObject.foot.position).normalized;
        MoveVelocity(direction,1f);

        float distance = Vector2.Distance(moveObject.foot.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            if (path.vectorPath.Count > currentWaypoint)
            {
                float dirForAnim = path.vectorPath[currentWaypoint].x - path.vectorPath[currentWaypoint - 1].x;
                TurnTowards(dirForAnim < 0);
            }
        }
    }
    public void SetPlayer(MoveObject moveObject)
    {
        if (moveObject == null)
        {
            state = EnemyState.Ori;
            CancelInvoke(nameof(UpdatePath));
            MoveVelocity(Vector2.zero, 0f);
            path = null;
            if(weaponDir)
                weaponDir.DisableUpdate();
            return;
        }
        Player = moveObject;
        
        if (Player.type == MoveObjectType.Dead)
        {
            state = EnemyState.Run;
        }
        else
        {
            state = EnemyState.toPlayer;
            Target = Player.transform.position;
            InvokeRepeating(nameof(UpdatePath), 0, 0.5f);
            if (weaponDir)
                weaponDir.BackToUpdate();
        }
    }
    public void NoFindPlayer()
    {
        Player = null;
        state = EnemyState.Ori;
        CancelInvoke(nameof(UpdatePath));
    }
    private void OnDisable()
    {
        moveObject.fearTex.SetActive(false);
    }
    private void OnDestroy()
    {
        moveObject.fearTex.SetActive(false);
    }
}
