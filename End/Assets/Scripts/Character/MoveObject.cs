﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public MoveObjectType type;

    [Header("攻击")]
    public float attackRadius = 5f;
    private AttackTiming attackTiming;
    [Tooltip("弹匣容量，默认为0（近战）")]
    public int bulletNum = 0;
    [Tooltip("换弹时间,默认为0（近战）")]
    public float shotReload = 0f;
    public float playerAttackSpace = 1f;
    [Tooltip("单次攻击间隔")]
    public float attackSpace = 2f;

    public float FearRadius = 5f;
    //控制图片整体，控制朝向
    [Header("图片")]
    public Transform GFX;
    public Animator[] animators;
    public Transform foot;
    public Transform attackPoint;
    public GameObject fearTex;
    public float injureAnimDur = 2f;

    [Header("其他")]
    public float MaxHp = 100f;
    public float speed = 10f;

    protected float Hp;

    [HideInInspector]
    public bool isPlayer;
    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    public Controller controller;
    protected Material material_Body;
    protected Material material_Edge;
    protected float OriScale;

    //状态机
    public enum State
    {
        Normal,
        Injure,
        Dead
    }
    public State _State;
    private void Reset()
    {
        //rigidbody = gameObject.AddComponent<Rigidbody2D>();
        //collider = gameObject.AddComponent<Collider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        OriScale = transform.localScale.x;

        attackTiming = new AttackTiming(bulletNum, shotReload, attackSpace);
        _State = State.Normal;
        Hp = MaxHp;
        isPlayer = false;

        if (type == MoveObjectType.Dead) return;
        material_Body = new Material(Game.instance.RoleShader);
        material_Edge = new Material(Game.instance.RoleShader);
        SpriteRenderer[] sprites = GFX.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer renderer in sprites)
        {
            renderer.material = material_Body;
        }
        SpriteRenderer[] sprites1 = GFX.GetChild(1).GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in sprites1)
        {
            renderer.material = material_Edge;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPossess();//附身检测代码
        attackTiming.Update();
    }
    protected void CheckPossess()
    {
        if (type == MoveObjectType.Dead || _State==State.Dead)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(transform.position, mousePos) < 2f)
                {
                    Game.instance.playerController.Possess(this);
                    _State = State.Injure;
                }
            }
        }
    }
    public virtual bool MouseBtnLeft(Vector2 targetPos)
    {
        return CallAttack(targetPos);
    }
    public virtual void Attack(Vector2 target)
    {

    }
    public virtual bool MouseBtnRight(Vector2 targetPos)
    {
        Skill(targetPos);
        return false;
    }
    public virtual void Skill(Vector2 target)
    {

    }
    public void MoveUpdate(Vector2 dir , float speedScale)
    {
        if (_State == State.Normal)
        {
            Vector3 detal = (Vector3)dir * speed * speedScale * Time.deltaTime;
            transform.position += detal;
        }
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="dirXscale"></param>
    /// <param name="animSpeed"></param>
    public void MoveVelocity(Vector2 dirXscale , float animSpeed)
    {
        if(_State == State.Normal)
        {
            rigidbody.velocity = speed * dirXscale;
            SetAnimSpeed(animSpeed);
            SetAnimLayerWeight(Mathf.Floor(animSpeed));
        }
    }
    public virtual void TurnTowards(bool isleft)
    {
        if(_State == State.Normal)
        {
            GFX.localScale = new Vector3(isleft ? 1 : -1, 1, 1);
        }
    }
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="value"></param>
    /// <param name="force"></param>
    public void GetHurt(float value,Vector2 force)
    {

        Hp -= value;
        if (Hp > 0)
        {
            PlayAnim("Injure");
            if (isPlayer)
            {
                collider.enabled = false;
                StartCoroutine(nameof(PlayerInjured));
                Invoke(nameof(AnimaInjureFinish), injureAnimDur);
                ((PlayerController)controller).GetHurtEffect(force);
            }
            else
            {
                SetAnimLayerWeight(0f);
                StartCoroutine(nameof(EnemyInjured));
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(force*10);
            }
        }
        else
        {
            PlayAnim("dead");
            SetAnimLayerWeight(0f);

            _State = State.Dead;
            if (isPlayer)
            {
                //Player dead;
                ((PlayerController)controller).MoveObjectDead();
                Destroy(gameObject);
            }
            else
            {
                _State = State.Dead;
                //
                Game.instance.CheckIfPass();
                collider.enabled = false;
                rigidbody.velocity = Vector2.zero;
                fearTex.SetActive(false);

                controller.enabled = false;
                material_Body.SetVector("_Color1", new Vector4(0.6792453f, 0.6792453f, 0.6792453f, 1));
                material_Edge.SetVector("_Color1", new Vector4(0, 0.9441266f, 1,1));
            }
        }
    }
    public void AnimaInjureFinish()
    {
        collider.enabled = true;
    }
    public void AnimaDeadFinish()
    {

    }
    protected IEnumerator DeadNoPossess()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
    public void PlayerLeaveThisBody()
    {
        PlayAnim("dead");
        MoveVelocity(Vector2.zero, 0f);
        Invoke(nameof(DestroySelf), 3f);
    }
    protected void DestroySelf()
    {
        Destroy(gameObject);
    }
    public IEnumerator PlayerInjured()
    {
        float amount = 1f;
        for (int i = 0; i < 12; i++)
        {
            amount = i % 2 == 1 ? 0f : 1f;
            material_Body.SetVector("_Color", amount * Vector4.one);
            yield return new WaitForSeconds(0.1f);
        }
        material_Body.SetVector("_Color", Vector4.one);
    }
    public IEnumerator EnemyInjured()
    {
        material_Body.SetFloat("_Shine", 0.5f);
        yield return new WaitForSeconds(0.1f);
        material_Body.SetFloat("_Shine", 0f);
    }
    /// <summary>
    /// 附身设置
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="isPlayer"></param>
    public void SetController(Controller controller , bool isPlayer)
    {
        this.controller = controller;
        this.isPlayer = isPlayer;
        if (isPlayer)
        {
            Game.instance.InformEnemie(this);
            Hp = 100;
            gameObject.tag = "Player";
            Destroy(GetComponent<EnemyController>());
            if (_State == State.Injure)
            {
                PlayAnim("dead2idle");
            }
            collider.enabled = true;
        }
        _State = State.Normal;

    }
    public Rigidbody2D GetRigidBody()
    {
        return rigidbody;
    }
    public void PlayAnim(string trigger)
    {
        foreach(Animator animator in animators)
        {
            animator.SetTrigger(trigger);
        }
    }
    public void PlayAnim(string anim,bool bo)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool(anim,bo);
        }
    }
    public void SetAnimSpeed(float speed)
    {
        foreach (Animator animator in animators)
        {
            animator.SetFloat("speed", speed);
        }
    }
    public void SetAnimLayerWeight(float weight)
    {
        foreach (Animator animator in animators)
        {
            animator.SetLayerWeight(1, weight);
        }
    }
    public bool CallAttack(Vector2 target)
    {
        int check = attackTiming.AttackCheck();

        switch (check)
        {
            case 0:
                break;
            case 1:
                Attack(target);
                return true;
            case 2:
                break;
            case 3:
                PlayAnim("reload");
                //开始换弹动画
                break;
        }
        return false;
    }
}

/// <summary>
/// 死物还是活物
/// </summary>
public enum MoveObjectType
{
    Living,
    Dead
}

