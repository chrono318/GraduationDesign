using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SeagullsMO : MoveObject
{
    [Header("海鸥")]
    public GameObject clock;
    public GameObject dizzy;
    public float hurtRadius = 5f;
    public float damage = 100f;
    [Tooltip("冲刺时离多近会爆炸")]
    public float preBoomRadius = 3f;
    private bool preBoom = false;
    public float rushTime = 3f;
    public float rushSpeed = 20f;
    [Tooltip("技能加速，加速到Speed（不是rushSpeed）的几倍")]
    public float skillSpeedScale = 1.5f;
    [Tooltip("冲击时方向改变的幅度")]
    public float rushDirChangeScale = 0.1f;
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
        while (t < rushTime)
        {
            Vector2 toplayer = PlayerController.instance.GetMoveObject().foot.position - foot.position;
            if(toplayer.magnitude <= preBoomRadius)
            {
                Boom();
                yield break;
            }
            if (hasCollide)
            {
                yield break;
            }
            dir += toplayer.normalized * rushDirChangeScale;
            MoveUpdate(dir, rushSpeed / speed);
            TurnTowards(dir.x < 0);
            t += Time.deltaTime;
            yield return null;
        }
        isRush = false;
        //休息
        PlayAnim("animState", 0);
    }
    /// <summary>
    /// 撞到墙体会眩晕
    /// </summary>
    public void CollideWhenRush(string collisionTag)
    {
        print(collisionTag + " isRush:"+isRush);
        if (isRush)
        {
            hasCollide = true;
            PlayAnim("animState", 3);
        }
    }

    //附身后
    void PlayerAttack(Vector2 targetPos)
    {
        StartCoroutine(nameof(PlayerRush));
    }
    IEnumerator PlayerRush()
    {
        float t = 0f;
        while (t < 5f)
        {
            t += Time.deltaTime;
            yield return null;
            if (Input.GetMouseButtonDown(0))
            {
                Boom();
                yield break;
            }
        }
        Boom();
    }
    public override void SetPlayer()
    {
        BtnDownF = new BtnDownDelegate(PlayerAttack);
    }
    private void OriUpdate()
    {
        DefaultUpdate();

        if (clock.activeSelf)
        {
            Clock();
        }
        if (isPlayer) return;
        MoveObject player = Game.instance.playerController.GetMoveObject();
        if (player == null) return;
        if (player.type==MoveObjectType.Dead) return;
        if (Vector2.Distance(player.transform.position, rigidbody.position) < preBoomRadius)
        {
            if (!preBoom) {
                preBoom = true;
                PlayAnim("animState", 4);
                return;
            }
            //Boom
            Boom();
        }
        
    }
    void Boom()
    {
        PlayAnim("animState", 2);
        clock.SetActive(false);
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
            Game.instance.CheckIfPass();
            Game.instance.DeleteEnemyMO(this);
        }
        Invoke(nameof(DestroySelf), 2f);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void Clock()
    {
        clock.transform.Find("CD").GetComponent<Image>().fillAmount -= -1.0f / 5f * Time.deltaTime;
        //指针旋转
        clock.transform.Find("Pointer").gameObject.transform.rotation = Quaternion.Euler(0, 0, 90 + clock.transform.Find("CD").GetComponent<Image>().fillAmount * -360);
    }
    IEnumerator Rush(Vector2 dir)
    {
        Vector2 oriPos = rigidbody.position;
        float dis = 0;
        MoveVelocity(dir * rushSpeed,1);
        while (dis < rushTime)
        {
            dis = (rigidbody.position - oriPos).magnitude;
            yield return 0;
        }
        //休息
        _State = State.Injure;
        PlayAnim("animState", 0);
        Invoke(nameof(AnimaInjureFinish), 3f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer==LayerMask.NameToLayer("Obstacles") && isRush)
        {
            //眩晕
            PlayAnim("animState", 3);
            _State = State.Injure;
            Invoke(nameof(AnimaInjureFinish), 3f);
        }
    }
}
