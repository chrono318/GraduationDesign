using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    private bool isPossess = false;
    public float ghostSpeed = 10f;
    [Tooltip("跑动时玩家速度/原物体速度")]
    public float runSpeedScale = 1.5f;
    public GameObject GFX;

    private CameraControl cameraControl;
    private float OriScale = 1f;
    [HideInInspector]
    public float San = 100f;
    // Start is called before the first frame update
    void Start()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        OriScale = transform.localScale.x;
    }
    
    // Update is called once per frame
    void Update()
    {

        
    }
    private void FixedUpdate()
    {
        //扣血/加血
        if (isPossess)
        {
            if(curType == MoveObjectType.Living)
            {
                San += 0.05f;
                San = Mathf.Min(San, 100f);
            }
            else
            {
                San -= 0.1f;
                if (San <= 0)
                {
                    // lose
                }
            }
        }
        else
        {
            San -= 0.1f;
            if (San <= 0)
            {
                // lose
            }
        }
   

        //移动
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dirSpeed = new Vector2(h, v);

        //朝向
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - transform.position;

        if (isPossess)
        {
            float animSpeed = dirSpeed.magnitude > 0.01 ? 1 : 0;
            //MoveUpdate(dirSpeed, runSpeedScale);
            MoveVelocity(dirSpeed * runSpeedScale, animSpeed);
            TurnTowards(dir.x < 0);
        }
        else
        {
            Vector3 detal = (Vector3)dirSpeed * ghostSpeed * Time.deltaTime;
            transform.position += detal;
            transform.localScale = new Vector3((dir.x < 0 ? 1 : -1)*OriScale, OriScale, 1);
        }

        if (!isPossess) return;
        //攻击（左键）/技能（右键）
        if (Input.GetMouseButton(0))
        {
            if (moveObject.MouseBtnLeft(mousePos))
            {
                Camera.main.GetComponent<CameraControl>().CameraShake(dir.normalized);//射击震动
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (moveObject.MouseBtnRight(mousePos))
            {
                //翻滚后效？
            }
        }
    }
    public void GetHurtEffect(Vector2 force)
    {
        cameraControl.CameraShake(force.normalized);
    }
    /// <summary>
    /// 附身对象被击杀
    /// </summary>
    public void MoveObjectDead()
    {
        isPossess = false;
        GFX.SetActive(true);

        transform.SetParent(moveObject.transform.parent, true);

        moveObject = null;
        GetComponent<Collider2D>().enabled = true;

    }
    /// <summary>
    /// 附身协程
    /// </summary>
    /// <param name="moveObject"></param>
    /// <returns></returns>
    private IEnumerator IEPossess(MoveObject moveObject)
    {
        yield return new WaitForSeconds(2f);
        isPossess = true;
        GFX.SetActive(false);
        SetMoveObject(moveObject,true);
        moveObject.isPlayer = true;

        GetComponent<Collider2D>().enabled = false;
        transform.SetParent(moveObject.transform);
        transform.localPosition = Vector3.zero;
    }
    public void Possess(MoveObject moveObject)
    {
        StartCoroutine(IEPossess(moveObject));
    }
}
