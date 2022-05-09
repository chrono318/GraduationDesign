using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.U2D.IK;

public class PlayerController : Controller
{
    public static PlayerController instance;
    private bool isPossess = false;
    private bool isRolling = false;
    public float ghostSpeed = 10f;
    [Tooltip("跑动时玩家速度/原物体速度")]
    public float runSpeedScale = 1.5f;
    public GameObject GFX;
    public Slider HPSlider;
    [Tooltip("每隔几秒扣除/增加一次San值")]
    public float SanTime = 0f;
    [Tooltip("每次扣除/增加多少San值")]
    public Vector2 SanPerTime = new Vector2(10, 5);

    private CameraControl cameraControl;
    private float OriScale = 1f;
    [HideInInspector]
    public float San = 100f;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        OriScale = transform.localScale.x;
        //InvokeRepeating(nameof(SanUpdate), 0, SanTime);
    }
    
    // Update is called once per frame
    void Update()
    {
        SanUpdate();

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
            moveObject.MoveVelocity(dirSpeed * runSpeedScale, animSpeed, dir.x < 0);
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
            if (moveObject.MouseBtnLeftDown(mousePos))
            {
                if (weaponDir)
                {
                    weaponDir.AttackShake(0.2f, 0.05f, mousePos);
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            moveObject.MouseBtnRightDown(mousePos);
        }

        if (weaponDir)
        {
            weaponDir.target = mousePos;
        }
    }
    private IEnumerator CamereaShake(Vector2 dirXForce, float shakeTime,float waitTime)
    {
        if (waitTime > 0)
        {
            yield return new WaitForSeconds(waitTime);
        }
        cameraControl.CameraShakeShot(dirXForce, shakeTime);//射击震动
    }
    /// <summary>
    /// 几秒后开始震动屏幕(单次震动)
    /// </summary>
    /// <param name="dirXForce">震动方向*强度,默认强度用normalized</param>
    /// <param name="waitTime">几秒后开始震动</param>
    public void CameraShakeShot(Vector2 dirXForce ,float shakeTime,float waitTime=0f)
    {
        StartCoroutine(CamereaShake(dirXForce, shakeTime, waitTime));
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
        if(InformOutPossessEvent!=null)
            InformOutPossessEvent();
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
        
        yield return new WaitForSeconds(1f);

        if (moveObject.type == MoveObjectType.Living)
        {
            weaponDir = moveObject.controller.weaponDir ?? null;
            if (weaponDir)
            {
                //weaponDir.enabled = true;
                //weaponDir.controller = this;
                weaponDir.GetComponent<IKManager2D>().enabled = true;
            }
            if (InformPossessEvent != null)
            {
                InformPossessEvent(moveObject);
            }
        }

        isPossess = true;
        GFX.SetActive(false);
        SetMoveObject(moveObject,true);
        moveObject.isPlayer = true;

        GetComponent<Collider2D>().enabled = false;
        transform.SetParent(moveObject.transform);
        transform.localPosition = Vector3.zero;
        Game.instance.FuShenVXF.position = moveObject.foot.position;
        Game.instance.FuShenVXF.GetChild(0).GetComponent<ParticleSystem>().Play();
        Game.instance.FuShenVXF.GetChild(1).GetComponent<ParticleSystem>().Play();

        isPossessing = false;
    }
    bool isPossessing = false;
    public void Possess(MoveObject moveObject)
    {   if (isPossessing) return;
        isPossessing = true;
        StartCoroutine(IEPossess(moveObject));
    }
    public delegate void InformPossess(MoveObject moveObject);
    public event InformPossess InformPossessEvent;
    public delegate void InformOutMO();
    public event InformOutMO InformOutPossessEvent;
}
