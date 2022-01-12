using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Pathfinding;

public class Enemy : Role
{
    public float speed4Player = 10;
    [Header("Paramater")]
    public float HP_Max = 100;
    [Tooltip("护甲值")]
    public float ArmorValue = 5;

    public Slider HP_Slider;
    public Slider HP_Slider_Bg;
    [HideInInspector]
    public float curHP;

    [HideInInspector]
    public GameObject ghost;
    //[Header("巡逻点")]
    //public List<Transform> PatrolPoints;
    [HideInInspector]
    public FSMcreator fSMcreator;

    
    [HideInInspector]
    public Seeker seeker;
    [HideInInspector]
    public Player target;
    [HideInInspector]
    public EnemyAI enemyAI;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        if(!TryGetComponent<EnemyAI>(out enemyAI))
        {
            Debug.LogWarning("No EnemyAI:" + this.gameObject.name);
        }
        else
        {
            enemyAI.GFX = GFX;
            enemyAI.speed = this.speed;
        }

        //HP_Slider.value = 1;
        curHP = HP_Max;

        fSMcreator = GetComponent<FSMcreator>();
        machine = fSMcreator.CreateSFM();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        fSMcreator.timing.Update();
    }
    private void FixedUpdate()
    {
        if (machine != null)
        {
            machine.Update();
        }
    }
    public virtual void MoveToPlayer()
    {
        if (!target) return;
    }
    public virtual void Attack()
    {
        print("attack!!!");
    }
    public virtual void BeHit(float damage)
    {
        float d = Mathf.Max(damage - ArmorValue, 0);
        UI_DamageManager.instance.DamageNum(d, DamageType.Physical,transform);
        curHP -= d;
        if (curHP <= 0)
        {
            Destroy(this.gameObject);
        }
        HP_Slider.value = curHP / HP_Max;
        HP_Slider_Bg.DOValue(curHP / HP_Max, 0.7f);
    }
    public override void GetHurt(float value, RoleType attackType,Vector2 force)
    {
        base.GetHurt(value, attackType,force);
        float d = Mathf.Max(value - ArmorValue, 0);
        //UI_DamageManager.instance.DamageNum(d, DamageType.Magic, transform);
        StartCoroutine(nameof(EnemyInjured));
        enemyAI.GetHurt = true;
        //受伤状态
        machine.TranslateToState(1);
        StartCoroutine(nameof(goon));
        //击退
        GetComponent<Rigidbody2D>().AddForce(force*500);

    }
    bool isDead = false;
    public override void Dead()
    {
        base.Dead();
        //
        machine.TranslateToState(1);
        machine.KillStateMachine();
        Destroy(enemyAI);
        //击退效果
        isDead = true;
        GetComponent<Rigidbody2D>().AddForce(deadDir * 10000);
    }
    private bool tanFirst = true;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead && tanFirst)
        {
            GetComponent<Rigidbody2D>().AddForce(collision.relativeVelocity*100);
            tanFirst = false;
        }
    }
    public void ShowHurt()
    {
        //animator.SetTrigger("injure");
        PlayAnima("injure");
        enemyAI.GetHurt = true;
        StartCoroutine(nameof(goon));
    }
    //受伤停顿，继续巡逻
    private IEnumerator goon()
    {
        yield return new WaitForSeconds(1.2f);
        enemyAI.GetHurt = false;
    }
    //public void TranslateToState(EnemyBaseState state)
    //{
    //    currentState = state;
    //    currentState.EnterState(this);
    //}
    public void SetAITarget(Transform pos)
    {
        enemyAI.Target = pos;
    }
    public void OnFinishLookAround()
    {

    }
    //发现敌人
    public void FindPlayer(Player player)
    {
        this.target = player;
        machine.TranslateToState(2);
    }
    //
    public void LoseTarget()
    {
        this.target = null;
        machine.TranslateToState(0);
    }

    public bool hasTarget { get => target ? true : false; }

    public void CloseAI()
    {
        enemyAI.enabled = false;
        //animator.SetFloat("speed", 0f);
    }
    public void OpenAI()
    {
        enemyAI.enabled = true;
    }
    //
    private void OnDestroy()
    {
        Game.instance.CheckIfPass();
        machine = null;
    }
}
