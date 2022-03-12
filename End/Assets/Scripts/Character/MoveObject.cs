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
    [Tooltip("单次攻击间隔")]
    public float attackSpace = 2f;

    public float FearRadius = 5f;
    //控制图片整体，控制朝向
    [Header("图片")]
    public Transform GFX;
    public Animator[] animators;

    [Header("其他")]
    public float MaxHp = 100f;
    public float speed = 10f;

    protected float Hp;

    [HideInInspector]
    public bool isPlayer;
    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    protected Controller controller;
    protected Material material;

    //状态机
    public enum State
    {
        Normal,
        Injure,
        Dead
    }
    protected State _State;
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

        attackTiming = new AttackTiming(bulletNum, shotReload, attackSpace);
        _State = State.Normal;
        Hp = MaxHp;
        isPlayer = false;

        material = new Material(Game.instance.RoleShader);
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer renderer in sprites)
        {
            renderer.material = material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual bool MouseBtnLeft()
    {
        return false;
    }
    public virtual bool MouseBtnRight()
    {
        return false;
    }
    public void MoveUpdate(Vector2 dir , float speedScale)
    {
        if (_State == State.Normal)
        {
            Vector3 detal = (Vector3)dir * speed * speedScale * Time.deltaTime;
            transform.position += detal;
        }
    }
    public void MoveVelocity(Vector2 dirXscale)
    {
        if(_State == State.Normal)
        {
            rigidbody.velocity = speed * dirXscale;
            PlayAnim("move", true);
        }
    }
    public virtual void TurnTowards(bool isleft)
    {
        if(_State == State.Normal)
        {
            GFX.localScale = new Vector3(isleft ? 1 : -1, 1, 1);
        }
    }

    bool isUnmatched = false;
    public void GetHurt(float value,Vector2 force)
    {

        Hp -= value;
        if (Hp > 0)
        {
            PlayAnim("Injure");
            if (isPlayer)
            {
                _State = State.Injure;
                collider.enabled = false;
                StartCoroutine(nameof(PlayerInjured));
                ((PlayerController)controller).GetHurtEffect(force);
            }
            else
            {
                StartCoroutine(nameof(EnemyInjured));
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(force*10);
            }
        }
        else
        {
            PlayAnim("Dead");
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
                Game.instance.playerController.AddDeadObject(this);
                Game.instance.CheckIfPass();
            }
        }
    }
    public void AnimaInjureFinish()
    {
        _State = State.Normal;
        collider.enabled = true;
    }
    public void AnimaDeadFinish()
    {

    }
    protected IEnumerator DeadNoPossess()
    {
        yield return new WaitForSeconds(3f);
        Game.instance.playerController.RemoveDeadObject(this);
        Destroy(this.gameObject);
    }
    public IEnumerator PlayerInjured()
    {
        float amount = 1f;
        for (int i = 0; i < 12; i++)
        {
            amount = i % 2 == 1 ? 0f : 1f;
            material.SetVector("_Color", amount * Vector4.one);
            yield return new WaitForSeconds(0.1f);
        }
        material.SetVector("_Color", Vector4.one);
    }
    public IEnumerator EnemyInjured()
    {
        material.SetFloat("_Shine", 0.5f);
        yield return new WaitForSeconds(0.1f);
        material.SetFloat("_Shine", 0f);
    }
    public void SetController(Controller controller)
    {
        this.controller = controller;
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
}

/// <summary>
/// 死物还是活物
/// </summary>
public enum MoveObjectType
{
    Living,
    Dead
}

