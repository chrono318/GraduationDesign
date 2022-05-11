using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SeagullsMO : MoveObject
{
    [Header("海鸥")]
    [Tooltip("进入爆炸准备状态的距离")]
    public float preBoomRadius = 3f;
    [Tooltip("直接触发爆炸的距离")]
    public float boomRadius = 1f;
    public float hurtRadius = 5f;
    public float damage = 100f;
    private bool preBoom = false;
    public float rushTime = 3f;
    public float rushSpeed = 20f;
    [Tooltip("技能加速，加速到Speed（不是rushSpeed）的几倍")]
    public float skillSpeedScale = 1.5f;
    [Tooltip("冲击时方向改变的幅度")]
    public float rushDirChangeScale = 0.1f;

    public GameObject clock;
    public Image clockImage;
    public Transform clockPointer;
    public GameObject dizzy;

    bool isWaitForBtn = false;
    bool isRush = false;
    bool hasCollide = false;

    private delegate void BtnDownDelegate(Vector2 targetPos);
    private BtnDownDelegate BtnDownF;
    protected override void Start()
    {
        base.Start();
        BtnDownF = new BtnDownDelegate(EnemyAttack);
    }

    public override bool MouseBtnLeftDown(Vector2 targetPos)
    {
        if (isRush) return false;
        BtnDownF(targetPos);
        return false;
    }
    
    void EnemyAttack(Vector2 targetPos)
    {
        isRush = true;
        StartCoroutine(EnemyRush(targetPos - (Vector2)foot.position));
    }
    IEnumerator EnemyRush(Vector2 dir)
    {
        float t = 0f;
        PlayAnim("animState", 1);
        while (t < rushTime)
        {
            Vector2 toplayer = PlayerController.instance.GetMoveObject().foot.position - foot.position;
            if(toplayer.magnitude <= preBoomRadius) //进预警范围了
            {
                if(toplayer.magnitude <= boomRadius) //进爆炸范围了
                {
                    StartCoroutine(Boom());
                    yield break;
                }
                else
                {
                    if (!preBoom)
                    {
                        preBoom = true;
                        PlayAnim("animState", 4);
                    }
                }
            }

            if (hasCollide)  //撞东西了
            {
                if (preBoom)
                {
                    StartCoroutine(Boom());
                    yield break;
                }
                else
                {
                    //眩晕
                    StartCoroutine(Dizzy());
                    isRush = false;
                    preBoom = false;
                    yield break;
                }
            }
            dir += toplayer.normalized * rushDirChangeScale;
            MoveUpdate(dir, rushSpeed / speed);
            TurnTowards(dir.x < 0);
            t += Time.deltaTime;
            yield return null;
        }
        isRush = false;
        preBoom = false;
        //休息
        PlayAnim("animState", 0);
        _State = State.Injure;
        Invoke(nameof(AnimaInjureFinish), 5f);
    }
    /// <summary>
    /// 撞到墙体会眩晕,给别的脚本调用的
    /// </summary>
    public void CollideWhenRush(string collisionTag)
    {
        if (isRush)
        {
            hasCollide = true;
            //PlayAnim("animState", 3);
        }
    }

    IEnumerator Dizzy()
    {
        PlayAnim("animState", 3);
        dizzy.SetActive(true);
        _State = State.Injure;
        yield return new WaitForSeconds(3f);
        AnimaInjureFinish();
        dizzy.SetActive(false);
    }
    public override void GetHurt(float value, Vector2 force, bool forceAutoNormalize = true)
    {
        if (preBoom)
        {
            StartCoroutine(Boom());
            return;
        }
        base.GetHurt(value, force, forceAutoNormalize);
    }
    //附身后
    void PlayerAttack(Vector2 targetPos)
    {
        StartCoroutine(nameof(PlayerRush));
    }
    IEnumerator PlayerRush()
    {
        clock.SetActive(true);
        preBoom = true;
        PlayAnim("animState", 4);
        float t = 0f;
        while (t < 5f)
        {
            yield return null;
            t += Time.deltaTime;
            clockImage.fillAmount = t / 5f;
            clockPointer.rotation = Quaternion.Euler(0, 0, 90 + t / 5f * -360);
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Boom());
                yield break;
            }
        }

        StartCoroutine(Boom());
    }
    public override void SetPlayer()
    {
        BtnDownF = new BtnDownDelegate(PlayerAttack);
    }
    
    IEnumerator Boom()
    {
        PlayAnim("animState", 2);
        clock.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        if (isPlayer)
        {
            foreach (MoveObject mo in Game.instance.curEnemies)
            {
                if (mo != null)
                {
                    if (mo.type == MoveObjectType.Dead) continue;
                    if (Vector2.Distance(mo.transform.position, rigidbody.position)<hurtRadius)
                    {
                        mo.GetHurt(damage, ((Vector2)mo.transform.position - rigidbody.position).normalized*100);
                    }
                }
            }
            ((PlayerController)controller).MoveObjectDead();
        }
        else
        {
            MoveObject player = Game.instance.playerController.GetMoveObject();
            if (Vector2.Distance(player.transform.position, rigidbody.position) < hurtRadius && player._State != State.Roll && player.type==MoveObjectType.Living)
            {
                player.GetHurt(damage, ((Vector2)player.transform.position - rigidbody.position).normalized*100);
            }
            _State = State.Dead;
            Game.instance.DeleteEnemyMO(this);
        }
        Invoke(nameof(DestroySelf), 0.4f);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    protected override void RegisterCanying()
    {
        ShadowPool.instance.RegisterCanying(GFX.gameObject, 10);
    }
    protected override void Canying()
    {
        ShadowPool.instance.SetCanying(GFX);
    }
}
