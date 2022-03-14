using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : Controller
{
    private bool isPossess = false;
    public float ghostSpeed = 10f;
    [Tooltip("跑动时玩家速度/原物体速度")]
    public float runSpeedScale = 1.5f;
    public GameObject GFX;
    public Slider HPSlider;
    [Tooltip("每隔几秒扣除/增加一次San值")]
    public float SanTime = 2f;
    [Tooltip("每次扣除/增加多少San值")]
    public Vector2 SanPerTime = new Vector2(10, 5);

    private CameraControl cameraControl;
    private float OriScale = 1f;
    [HideInInspector]
    public float San = 100f;
    // Start is called before the first frame update
    void Start()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        OriScale = transform.localScale.x;
        InvokeRepeating(nameof(SanUpdate), 0, SanTime);
    }
    
    // Update is called once per frame
    void Update()
    {

        
    }
    /// <summary>
    /// 扣血/加血
    /// </summary>
    void SanUpdate()
    {
        if (isPossess)
        {
            if (curType == MoveObjectType.Living)
            {
                San += SanPerTime.y;
                San = Mathf.Min(San, 100f);
            }
            else
            {
                San -= SanPerTime.x;
                if (San <= 0)
                {
                    // lose
                }
            }
        }
        else
        {
            San -= SanPerTime.x;
            if (San <= 0)
            {
                // lose
            }
        }
        HPSlider.DOValue(San / 100f, 0.5f);
    }
    private void FixedUpdate()
    {
        
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
                //StartCoroutine(CamereaShake(dir, moveObject.shakeTime));
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
    public IEnumerator CamereaShake(Vector2 dir,float time)
    {
        yield return new WaitForSeconds(time);
        Camera.main.GetComponent<CameraControl>().CameraShake(dir.normalized);//射击震动
    }
    public void GetHurtEffect(Vector2 force)
    {
        cameraControl.CameraInjure(force.normalized);
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
        Game.instance.InformEnemie(null);
    }
    /// <summary>
    /// 附身协程
    /// </summary>
    /// <param name="moveObject"></param>
    /// <returns></returns>
    private IEnumerator IEPossess(MoveObject moveObject)
    {
        if (isPossess)
        {
            this.moveObject.controller = null;
            this.moveObject.isPlayer = false;
            if (this.moveObject.type == MoveObjectType.Living)
            {
                this.moveObject.gameObject.tag = "Enemy";
                this.moveObject.PlayerLeaveThisBody();
            }
        }
        yield return new WaitForSeconds(2f);
        isPossess = true;
        GFX.SetActive(false);
        SetMoveObject(moveObject,true);
        moveObject.isPlayer = true;

        GetComponent<Collider2D>().enabled = false;
        transform.SetParent(moveObject.transform);
        transform.localPosition = Vector3.zero;

        isPossessing = false;
    }
    bool isPossessing = false;
    public void Possess(MoveObject moveObject)
    {   if (isPossessing) return;
        isPossessing = true;
        StartCoroutine(IEPossess(moveObject));
    }
}
