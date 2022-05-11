using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Blade : MonoBehaviour
{
    public Transform startPoint, endPoint;
    Vector2 moveDir, startPos, endPos;
    public float speed = 0.01f;
    int toward = 1;
    public float stopTime = 1.5f;
    public float currStopTime;
    public bool isForward  = true;
    bool isReach;
    Vector2 targetPoint;

    void Start()
    {
        //调整位置
        if (isForward)
        {
            transform.position = startPoint.position;
        }
        else
        {
            transform.position = endPoint.position;
        }

        startPos = new Vector2(startPoint.position.x, startPoint.position.y);
        endPos = new Vector2(endPoint.position.x, endPoint.position.y);
        currStopTime = stopTime;
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            if (!isReach)
            {
                Movement();
            }
             StopMove();
        }
        else
        {
            //调整位置
            if (isForward)
            {
                transform.position = startPoint.position;
            }
            else
            {
                transform.position = endPoint.position;
            }
        }
    }

    /// <summary>
    /// 选取目标点
    /// </summary>
    public void SelectTargetPoint()
    {
        if (isForward)
        {
            toward = 1;
            targetPoint = endPos;
        }
        else
        {
            toward = -1;
            targetPoint = startPos;
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    public void Movement()
    {
        SelectTargetPoint();
        //移动
        transform.Translate(Vector3.right * speed * toward);
        if(Mathf.Abs(transform.position.x - targetPoint.x) + Mathf.Abs(transform.position.y - targetPoint.y) < 0.1f)
        {
            isReach = true;
        }        
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        if (isReach)
        {
            currStopTime -= Time.deltaTime;
            if(currStopTime <= 0)
            {
                isReach = false;
                isForward = !isForward;
                currStopTime = stopTime;
                SoundManager.instance.PlaySoundClip(SoundManager.instance.effectSound[8]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MoveObject>(out MoveObject mo))
        {
            collision.gameObject.GetComponent<MoveObject>().GetHurt(20f, new Vector2(0, 0));
        }
    }
}
