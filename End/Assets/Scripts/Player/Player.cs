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
    }

    // Update is called once per frame
    void Update()
    {
        fsmCreator.timing.Update();

        if (!canMove) return;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float t = Time.deltaTime;
        Vector3 trans = new Vector3(h * speed*1.5f * t, v * speed*1.5f * t, 0);
        transform.position += trans;
        //朝向
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        float dir1 = dir.x < 0 ? 1 : -1;
        transform.GetChild(0).localScale = new Vector3(dir1, 1, 1) ;
        shadow.localPosition = new Vector3(dir1*shadowOriPos.x, shadowOriPos.y, 0);

        transform.GetChild(0).GetComponent<Animator>().SetFloat("speed", Mathf.Clamp01(Mathf.Abs(v * v + h * h) * 2));

        if (Input.GetMouseButtonDown(0))
        {
            if (fsmCreator.attackState.CallAttack())
            {
                Camera.main.GetComponent<CameraControl>().CameraShake(dir.normalized);//射击震动
            }
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            Dead();
        }
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
