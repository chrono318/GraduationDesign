using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private Transform target;
    [HideInInspector]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform GFX;

    Path path;
    int currentWaypoint = 0;
    public UnityEvent OnReach;
    private bool Reached = true;
    [HideInInspector]
    public bool GetHurt = false;
    Seeker seeker;
    Rigidbody2D rigid;

    public float speed_anim = 0.5f;
    [HideInInspector]
    public Transform shadow;
    [HideInInspector]
    public Vector3 shadowOriPos;
    public Transform foot;
    //
    private float scale = 1f;
    private float dirForAnim = 0f;

    public Transform Target { get => target;
        set 
        {
            if(target != value)
            {
                target = value;
                
                Reached = false;
            }
            if (!seeker)
                seeker = GetComponent<Seeker>();
            //UpdatePath();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigid = GetComponent<Rigidbody2D>();
        //InvokeRepeating("UpdatePath", 0f, .5f);
        scale = transform.localScale.x;

        shadowOriPos = shadow.transform.localPosition;
    }
    public void StartMove()
    {
        InvokeRepeating(nameof(UpdatePath), 0, 0.5f);
    }
    public void StopMove()
    {
        CancelInvoke(nameof(UpdatePath));
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            //seeker.StartPath(rigid.position, Target.position, OnPathComplete);
            seeker.StartPath(foot.position, Target.position, OnPathComplete);
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
    public void FixedUpdate()
    {
        
        if (path == null) return;
        if (Reached || GetHurt) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            OnReach.Invoke();
            Reached = true;
            //GFX.GetComponent<Animator>().SetFloat("speed",0f);
            GetComponent<Enemy>().SetAnimaSpeed(0f);
            return;
        }
        else
        {
            Reached = false;
        }
        

        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigid.position).normalized;
        Vector2 direction = (path.vectorPath[currentWaypoint] - foot.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime * speed_anim;

        //rigid.AddForce(force);
        transform.Translate(force);

        //anima
        //float speed1 = direction.magnitude > 0.0001f ? 1f : 0f;
        //speed_anim *= speed1;

        //GFX.GetComponent<Animator>().SetFloat("speed", speed_anim);
        GetComponent<Enemy>().SetAnimaSpeed(speed_anim);
        //transform.localScale = new Vector3(force.x > 0 ? -1 : 1, 1, 1);

        //float distance = Vector2.Distance(rigid.position, path.vectorPath[currentWaypoint]);
        float distance = Vector2.Distance(foot.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            if (path.vectorPath.Count == 1) return;
            dirForAnim = path.vectorPath[currentWaypoint].x - path.vectorPath[currentWaypoint - 1].x;
        }
        //图片转向
        if (dirForAnim >= 0.01)
        {
            GFX.localScale = new Vector3(-1, 1, 1) * scale;
            shadow.localPosition = new Vector3(-1 * shadowOriPos.x, shadowOriPos.y, 0);
        }
        else if (dirForAnim <= -0.01)
        {
            GFX.localScale = new Vector3(1, 1, 1) * scale;
            shadow.localPosition = new Vector3( shadowOriPos.x, shadowOriPos.y, 0);
        }
    }
}
