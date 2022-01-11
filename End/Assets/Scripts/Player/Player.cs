using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Role
{
    public Transform leftHand;
    public Transform rightHand;
    public bool canMove = true;

    private Player changeTarget;
    public Gun gun;

    public Ghost ghost;

    public FSMcreator fsmCreator;
    //
    private Collider2D collider;

    Transform shadow;
    Vector3 shadowOriPos;

    public enum State
    {
        run,
        attack,
        roll
    }
    public State state = State.run;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one;
        speed = GetComponent<Enemy>().speed4Player;
        //
        Game.instance.InformEnemie(this);

        Init();
        shadow = GetComponent<EnemyAI>().shadow;
        shadowOriPos = GetComponent<EnemyAI>().shadowOriPos;
        Destroy(GetComponent<Enemy>());
        Destroy(GetComponent<EnemyAI>());
        transform.GetChild(0).GetComponent<Animator>().SetFloat("speed", 1f);

        fsmCreator = GetComponent<FSMcreator>();
        fsmCreator.SetPlayer(this);

        GameObject.Instantiate(Game.instance.circle, transform.position, transform.rotation, transform.GetChild(0));

        collider = GetComponent<Collider2D>();

        state = State.run;
        m_speed = speed;
    }
    //speed固定不变，m_speed参加位移计算
    private float m_speed;
    [Header("翻滚速度（正常速度的倍数）")]
    public float rollSpeed = 1.5f;
    public float rollDuration = 0.5f;

    Vector2 roll_dir = Vector2.zero;
    // Update is called once per frame
    void Update()
    {
        fsmCreator.timing.Update();

        if (!canMove) return;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float t = Time.deltaTime;

        //朝向
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        float dir1 = dir.x < 0 ? 1 : -1;
        transform.GetChild(0).localScale = new Vector3(dir1, 1, 1);
        shadow.localPosition = new Vector3(dir1 * shadowOriPos.x, shadowOriPos.y, 0);

        transform.GetChild(0).GetComponent<Animator>().SetFloat("speed", Mathf.Clamp01(Mathf.Abs(v * v + h * h) * 2));


        if (Input.GetMouseButtonDown(0) && state == State.run)
        {
            if (fsmCreator.attackState.CallAttack())
            {
                Camera.main.GetComponent<CameraControl>().CameraShake(dir.normalized);//射击震动
                state = State.attack;
            }
        }
        if (Input.GetMouseButtonDown(1) && state == State.run)
        {
            state = State.roll;
            unmatched = true;
            roll_dir = dir.normalized;
            collider.enabled = false;
            Invoke(nameof(OnRollFinish), rollDuration);
        }

        Vector3 trans = Vector3.zero;

        switch (state)
        {
            case State.run:
                m_speed = speed;
                trans = new Vector3(h * m_speed * 1.5f * t, v * m_speed * 1.5f * t, 0);
                break;
            case State.attack:
                m_speed = 0;
                break;
            case State.roll:
                m_speed = speed * rollSpeed;
                trans = new Vector3(roll_dir.x * m_speed * 1.5f * t, roll_dir.y * m_speed * 1.5f * t, 0);
                break;
        }

        transform.position += trans;

        if(Input.GetKeyDown(KeyCode.I))
        {
           Dead();
        }
        
    }
    void OnRollFinish()
    {
        unmatched = false;
        roll_dir = Vector2.zero;
        state = State.run;
        collider.enabled = true;
    }
    public void OnAttackFinish()
    {
        state = State.run;
    }

    private void OnDestroy()
    {
        ghost.transform.SetParent(transform.parent);
        ghost.LeaveBodyForced();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gm in enemies)
        {
            gm.GetComponent<Enemy>().LoseTarget();
        }
    }

    private bool unmatched = false;//无敌
    public override void GetHurt(float value, RoleType attackType,Vector2 force)
    {
        if (unmatched) return;
        unmatched = true;
        collider.enabled = false;
        base.GetHurt(value, attackType,force);
        CameraControl control = Camera.main.GetComponent<CameraControl>();
        //control.CameraShake(force);
        control.CameraInjure(force);//受击震动，F12 CameraInjure函数
        StartCoroutine(nameof(PlayerInjured));
        Invoke(nameof(NotUnmatched), 2f);//无敌
    }
    void CanMove()
    {
        canMove = true;
    }
    void NotUnmatched()
    {
        unmatched = false;
        collider.enabled = true;
    }
}
