using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBoss : Boss
{
    private float _t = 0f;
    private Animator animator;
    enum BossState
    {
        idle,
        persuit,  //追击
        attack,
    }
    BossState _state = BossState.idle;
    [Header("普通攻击")]
    private bool isAttacking = false;
    public float attackCD = 10f;
    public float attackDamage = 10f;
    public ParticleSystem attackVFX;
    public ParticleSystem attackVFX1;
    public GameObject preAttackVFX;
    [Header("技能")]
    public float firstTime = 5f;
    public float skillCD = 10f;
    private int lastSkillIndex = 0;
    private bool isSkill = false;
    public Transform firePoint;
    [Tooltip("只播一个技能，-1代表随机，方便测试")]
    public int skillLock = -1;

    [Header("技能-冲击")]
    public float damageSkill0 = 30f;
    public float beginTime0 = 1f;
    public float endTime0 = 1f;
    public float rushDistance = 10f;
    public float rushDuration = 1f;
    public float rushAttackWidth = 3f;
    public ParticleSystem prerushVFX;
    public ParticleSystem rushVFX;
    [Header("技能-激光")]
    [Tooltip("扫描时间")]
    public float preTime1 = 3f;
    public float beginTime1 = 1f;
    public float endTime1 = 1f;
    public float damagePerSecond = 30f;
    public float widthSkill1 = 1f;
    public float maxLengthSkill1 = 10f;
    public float speedScaleSkill1 = 0.2f;
    public float step1Duration = 5f;
    public float gapDuration = 1f;
    public float step2Duration = 5f;
    public LineRenderer line0;
    public LineRenderer line1;
    public LineRenderer line2;
    public ParticleSystem prelaserVFX;
    public Material jiguangMat;
    public Material jiguangPreMat;
    [Header("技能-弹幕")]
    public float beginTime2 = 1f;
    public float endTime2 = 1f;
    public GameObject bullet;
    public float bulletDamage = 10f;
    //子弹池
    private Pool bulletPool;
    public float bulletSpeed = 5f;
    public float sumAngle = 60f;
    public float fireInterval = 0.3f;
    public float stepIntervalSkill2 = 2f;
    public int frequency = 5;
    public int bulletNumPerFire = 5;
    public int frequency1 = 7;
    public int bulletNumPerFire1 = 7;
    [Header("技能-飞天")]
    public float beginTime3 = 1f;
    public float endTime3 = 1f;
    public int jumpNumStep1 = 3;
    public int jumpNumStep2 = 5;
    public float stepIntervalSkill3 = 2f;
    public float intervalPerJump = 2f;
    public float timeUp = 1f;
    public float timeOn = 2f;
    public float timeDown = 1f;
    public float areaSkill3 = 3f;
    public float damageSkill3 = 30f;
    public SpriteRenderer shadow;
    public ParticleSystem upVFX;
    public ParticleSystem downVFX;
    public float minShadowSize = 0.5f;
    [Header("召唤小怪")]
    public int maxEnemyCount = 6;//间隔
    public List<GameObject> enemyPrafabs;
    public List<Transform> bornPoint;

    public ParticleSystem[] callEnemyVFXs;
    private int curEnemyCout;
    // Start is called before the first frame update
    void Start()
    {
        _state = BossState.idle;
        animator = animators[0];
        player = PlayerController.instance;
        player.InformPossessEvent += Player_InformPossessEvent;
        player.InformOutPossessEvent += Player_InformOutPossessEvent;

        bulletPool = PoolManager.instance.RegisterPool(bullet);
        bulletPool.registor.Add(gameObject);
        curEnemyCout = Game.instance.CurEnemyCount;
  
    }
    private void Player_InformPossessEvent(MoveObject target)
    {
        targetMO = target;
        InvokeRepeating(nameof(UpdatePath), 0, 0.5f);
        reached = false;
        Invoke(nameof(StartSkillTiming), firstTime);
    }
    private void Player_InformOutPossessEvent()
    {
        path = null;
        CancelInvoke(nameof(UpdatePath));
        reached = true;
        CancelInvoke(nameof(StartSkillTiming));
        CancelInvoke(nameof(ContinueSkillTiming));

        //StopAllCoroutines();
        reached = true;
        isAttacking = false;
        isSkill = false;
        animator.Play("laser(2)");
        targetMO = null;

        line0.enabled = false;
        line1.enabled = false;
        line2.enabled = false;
        line0.transform.GetChild(0).gameObject.SetActive(false);
        line0.transform.GetChild(1).gameObject.SetActive(false);
        line1.transform.GetChild(0).gameObject.SetActive(false);
        line1.transform.GetChild(1).gameObject.SetActive(false);
        line2.transform.GetChild(0).gameObject.SetActive(false);
        line2.transform.GetChild(1).gameObject.SetActive(false);
    }

    protected override void Dead()
    {
        line0.enabled = false;
        line1.enabled = false;
        line2.enabled = false;
        line0.transform.GetChild(0).gameObject.SetActive(false);
        line0.transform.GetChild(1).gameObject.SetActive(false);
        line1.transform.GetChild(0).gameObject.SetActive(false);
        line1.transform.GetChild(1).gameObject.SetActive(false);
        line2.transform.GetChild(0).gameObject.SetActive(false);
        line2.transform.GetChild(1).gameObject.SetActive(false);

        StopAllCoroutines();
        CancelInvoke();
        animator.SetLayerWeight(1, 0f);

        base.Dead();
    }

    // Update is called once per frame
    void Update()
    {
        //碰着掉血
        if (targetMO)
        {
            Vector2 dir = GetDirToPlayer();
            if (dir.magnitude < 2 && !inAir)
            {
                targetMO.GetHurt(attackDamage, dir);
            }
            if (!isSkill && !isAttacking)
            {
                if (dir.magnitude <= attackDis)
                {
                    StartCoroutine(nameof(Attack));
                }
            }
        } 
        curEnemyCout = Game.instance.CurEnemyCount;
        //召唤小怪的逻辑
        if (curEnemyCout <= 0)
        {
            if (player.GetMoveObject())
            {
                if (player.GetMoveObject().GetHP() > 50f)
                {
                    return;
                }
            }
            if (!isSkill)
            {
                StartSkill(4);
                //print("血少"+curEnemyCout);
            }
        }

    }

    private void FixedUpdate()
    {
        if (reached)
        {
            animator.SetLayerWeight(1,0f);
            animator.SetBool("move", false);
            return;
        }
        else
        {
            animator.SetBool("move", true);
            animator.SetLayerWeight(1, 1f);
        }
        animator.SetFloat("speed", speedScale);

        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = (path.vectorPath[currentWaypoint] - foot.position);

        transform.position += (Vector3)direction.normalized * Time.deltaTime * speed * speedScale;

        float distance = direction.magnitude;

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            //if (path.vectorPath.Count > currentWaypoint)
            //{
            //    float dirForAnim = path.vectorPath[currentWaypoint].x - path.vectorPath[currentWaypoint - 1].x;
            //    GFX.localScale = new Vector3(dirForAnim > 0 ? -1 : 1, 1, 1);
            //}
        }
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    IEnumerator Attack()
    {
        animator.SetTrigger("attack");
        preAttackVFX.SetActive(true);
        isAttacking = true;
        yield return new WaitForSeconds(1f); //动画里就是1s后挥击

        attackVFX.Play(true);
        attackVFX1.Play(true);
        preAttackVFX.SetActive(false);
        Vector2 dir = GetDirToPlayer();
        if (dir.magnitude <= attackDis)
        {
            targetMO.GetHurt(attackDamage, dir);
        }
        yield return new WaitForSeconds(attackCD - 1);
        isAttacking = false;
    }

    Vector2 GetAttackDir()
    {
        if(targetMO)
            return targetMO.transform.position - firePoint.position;
        return Vector2.zero;
    }

    void StartSkillTiming()
    {
        int index = Random.Range(0, 5);
        StartSkill(index);
    }
    void ContinueSkillTiming()
    {
        if (isSkill || !targetMO) return;
        int index = Random.Range(1, 5);
        index += lastSkillIndex;
        index %= 5;
        StartSkill(index);
    }
    void StartSkill(int index)
    {
        if (isSkill || !targetMO) return;
        isSkill = true;
        lastSkillIndex = index;
        //停止普通攻击
        StopCoroutine(nameof(Attack));
        isAttacking = false;
        preAttackVFX.SetActive(false);

        if (skillLock >= 0)
        {
            index = skillLock;
        }
        switch (index)
        {
            case 0:
                StartCoroutine(nameof(Skill0));
                break;
            case 1:
                StartCoroutine(nameof(Skill1));
                break;
            case 2:
                StartCoroutine(nameof(Skill2));
                break;
            case 3:
                StartCoroutine(nameof(Skill3));
                break;
            case 4:
                if (curEnemyCout < maxEnemyCount)
                {
                    StartCoroutine(nameof(Skill4));
                }
                else
                {
                    ContinueSkillTiming();
                }
                break;
        }
    }
    /// <summary>
    /// 冲撞
    /// </summary>
    IEnumerator Skill0()
    {
        //准备
        reached = true;
        animator.SetTrigger("rushPre");
        rushVFX.gameObject.SetActive(true);
        rushVFX.Play();
        prerushVFX.Play(true);
        yield return new WaitForSeconds(beginTime0);
        prerushVFX.Stop(true);

        //开撞
        float t = 0;
        Vector2 dir = GetDirToPlayer();
        float _speed = rushDistance / rushDuration;
        animator.SetTrigger("rush");
        
        
        while (t < rushDuration)
        {
            transform.position += (Vector3)dir.normalized * _speed * Time.deltaTime;
            t += Time.deltaTime;

            if (GetDirToPlayer().magnitude <= rushAttackWidth)
            {
                targetMO.GetHurt(damageSkill0, GetDirToPlayer() * 3 ,false);
            }
            yield return 0;
        }

        //结束动作
        
        yield return new WaitForSeconds(endTime0);

        reached = false;
        isSkill = false;
        rushVFX.gameObject.SetActive(false);
        animator.SetTrigger("rushEnd");
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    /// <summary>
    /// 激光
    /// </summary>
    IEnumerator Skill1()
    {
        speedScale = speedScaleSkill1;
        Vector2 dir;
        Vector2 dir1;
        Vector2 dir2;
        float t = 0;
        //扫描
        reached = true;
        line0.enabled = true;
        line0.material = jiguangPreMat;

        while (t < preTime1)
        {
            KeepStartPosJiguang();
            dir = GetAttackDir();
            Vector2 endPos = GetJiguangEndPos(dir);
            line0.SetPosition(1, endPos);
            t += Time.deltaTime;
            yield return null;
        }
        dir = GetAttackDir();
        for (int i = 0; i < 2; i++)
        {
            line0.enabled = false;
            yield return new WaitForSeconds(0.2f);
            line0.enabled = true;
            yield return new WaitForSeconds(0.2f);
            line0.enabled = false;
        }
        

        animator.SetTrigger("laserPre");
        prelaserVFX.Play(true);
        yield return new WaitForSeconds(beginTime1);
        prelaserVFX.Stop(true);
        //第一阶段
        reached = false;
        line0.enabled = true;
        line0.material = jiguangMat;

        line0.transform.GetChild(0).gameObject.SetActive(true);
        line0.transform.GetChild(1).gameObject.SetActive(true);
        line0.transform.GetChild(0).position = (Vector2)firePoint.position;
        line0.transform.GetChild(1).position = GetJiguangEndPos(dir);

        line0.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir);
        line0.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir);

        t = 0;
        bool bo = false;
        animator.SetTrigger("laser");
        while (t < step1Duration)
        {
            Vector2 endPos = GetJiguangEndPos(dir);
            KeepStartPosJiguang();
            line0.SetPosition(1, endPos);
            line0.transform.GetChild(1).position = endPos;
            if (Vector2.Dot(GetAttackDir(), dir.normalized) > 0 && GetAttackDir().magnitude < (endPos-(Vector2)firePoint.position).magnitude)
            {
                if ((GetAttackDir() - Vector2.Dot(GetAttackDir(), dir.normalized) * dir.normalized).magnitude <= widthSkill1 / 2)
                {
                    if (bo)   //上一帧就在范围内，结算伤害
                    {
                        targetMO.GetHurt(damagePerSecond, dir);
                    }
                    bo = true;
                }
                else
                {
                    bo = false;
                }
            }
            else
            {
                bo = false;
            }
            t += Time.deltaTime;
            yield return 0;
        }
        line0.enabled = false;
        line0.transform.GetChild(0).gameObject.SetActive(false);
        line0.transform.GetChild(1).gameObject.SetActive(false);
        animator.SetTrigger("laserEnd");
        reached = true;


        //间隔时间
        yield return new WaitForSeconds(gapDuration);
        //第二阶段扫描
        reached = true;
        line0.enabled = true;
        line0.material = jiguangPreMat;
        line1.enabled = true;
        line1.material = jiguangPreMat;
        line2.enabled = true;
        line2.material = jiguangPreMat;

        t = 0;
        while (t < preTime1)
        {
            KeepStartPosJiguang();
            dir = GetAttackDir();
            dir1 = Quaternion.Euler(0, 0, 45f) * dir;
            dir2 = Quaternion.Euler(0, 0, -45f) * dir;
            Vector2 endPos = GetJiguangEndPos(dir);
            line0.SetPosition(1, endPos);
            Vector2 endPos1 = GetJiguangEndPos(dir1);
            line1.SetPosition(1, endPos1);
            Vector2 endPos2 = GetJiguangEndPos(dir2);
            line2.SetPosition(1, endPos2);
            t += Time.deltaTime;
            yield return null;
        }
        dir = GetAttackDir();
        dir1 = Quaternion.Euler(0, 0, 45f) * dir;
        dir2 = Quaternion.Euler(0, 0, -45f) * dir;
        for (int i = 0; i < 2; i++)
        {
            line0.enabled = false;
            line1.enabled = false;
            line2.enabled = false;
            yield return new WaitForSeconds(0.2f);
            line0.enabled = true;
            line1.enabled = true;
            line2.enabled = true;
            yield return new WaitForSeconds(0.2f);
            line0.enabled = false;
            line1.enabled = false;
            line2.enabled = false;
        }
            
        animator.SetTrigger("laserPre");
        prelaserVFX.Play(true);
        yield return new WaitForSeconds(beginTime1);
        prelaserVFX.Stop(true);
        //第二阶段射
        reached = false;
        animator.SetTrigger("laser");
        line0.enabled = true;
        line0.material = jiguangMat;
        line1.enabled = true;
        line1.material = jiguangMat;
        line2.enabled = true;
        line2.material = jiguangMat;
        line0.transform.GetChild(0).gameObject.SetActive(true);
        line0.transform.GetChild(1).gameObject.SetActive(true);
        line1.transform.GetChild(0).gameObject.SetActive(true);
        line1.transform.GetChild(1).gameObject.SetActive(true);
        line2.transform.GetChild(0).gameObject.SetActive(true);
        line2.transform.GetChild(1).gameObject.SetActive(true);   

        line0.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir);
        line0.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir);
        line1.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir1);
        line1.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir1);
        line2.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir2);
        line2.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir2);

        bo = false;
        t = 0;
        while (t < step2Duration)
        {
            Vector2 endPos = GetJiguangEndPos(dir);
            Vector2 endPos1 = GetJiguangEndPos(dir1);
            Vector2 endPos2 = GetJiguangEndPos(dir2);

            KeepStartPosJiguang();
            line0.SetPosition(1, endPos);
            line1.SetPosition(1, endPos1);
            line2.SetPosition(1, endPos2);
            line0.transform.GetChild(1).position = endPos;
            line1.transform.GetChild(1).position = endPos1;
            line2.transform.GetChild(1).position = endPos2;

            if (Vector2.Dot(GetAttackDir(), dir.normalized) > 0 && GetAttackDir().magnitude < (endPos - (Vector2)firePoint.position).magnitude)
            {
                if ((GetAttackDir() - Vector2.Dot(GetAttackDir(), dir.normalized) * dir.normalized).magnitude <= widthSkill1 / 2
                || (GetAttackDir() - Vector2.Dot(GetAttackDir(), dir1.normalized) * dir1.normalized).magnitude <= widthSkill1 / 2
                || (GetAttackDir() - Vector2.Dot(GetAttackDir(), dir2.normalized) * dir2.normalized).magnitude <= widthSkill1 / 2)
                {
                    if (bo)   //上一帧就在范围内，结算伤害
                    {
                        targetMO.GetHurt(damagePerSecond, dir);
                    }
                    else  //不在的话
                    {
                        bo = true;
                    }
                }
                else
                {
                    bo = false;
                }
            }
            else
            {
                bo = false;
            }

            t += Time.deltaTime;
            yield return 0;
        }
        line0.enabled = false;
        line1.enabled = false;
        line2.enabled = false;
        line0.transform.GetChild(0).gameObject.SetActive(false);
        line0.transform.GetChild(1).gameObject.SetActive(false);
        line1.transform.GetChild(0).gameObject.SetActive(false);
        line1.transform.GetChild(1).gameObject.SetActive(false);
        line2.transform.GetChild(0).gameObject.SetActive(false);
        line2.transform.GetChild(1).gameObject.SetActive(false);
        //结束动作
        animator.SetTrigger("laserEnd");
        reached = true;
        yield return new WaitForSeconds(endTime1);
        reached = false;

        speedScale = 1f;
        isSkill = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    Vector2 GetJiguangEndPos(Vector2 curDir)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)firePoint.position, curDir, maxLengthSkill1, LayerMask.GetMask("Obstacles"));
        if (hit)
        {
            return hit.point;
        }
        else
        {
            return (Vector2)firePoint.position + curDir.normalized * maxLengthSkill1;
        }
    }
    void KeepStartPosJiguang()
    {
        line0.SetPosition(0, (Vector2)firePoint.position);
        line1.SetPosition(0, (Vector2)firePoint.position);
        line2.SetPosition(0, (Vector2)firePoint.position);
        line0.transform.GetChild(0).position = (Vector2)firePoint.position;
        line1.transform.GetChild(0).position = (Vector2)firePoint.position;
        line2.transform.GetChild(0).position = (Vector2)firePoint.position;
    }
    /// <summary>
    /// 弹幕
    /// </summary>
    /// <returns></returns>
    IEnumerator Skill2()
    {
        reached = true;
        animator.SetTrigger("firePre");
        yield return new WaitForSeconds(beginTime2);
        //第一波弹幕
        Vector2 dir = GetAttackDir();
        float angleOri = Vector2.SignedAngle(Vector2.right, dir);
        for (int i = 0; i < frequency; i++)
        {
            int offset = 0;
            for(int j = 0; j < bulletNumPerFire; j++)
            {
                offset += j * (j % 2 == 0 ? -1 : 1);
                Quaternion quaternion = Quaternion.Euler(0, 0, angleOri + sumAngle / (bulletNumPerFire - 1) * offset);
                CreateBullet(firePoint.position, quaternion);
            }
            yield return new WaitForSeconds(fireInterval);
        }

        yield return new WaitForSeconds(stepIntervalSkill2 - beginTime2);
        animator.SetTrigger("firePre");
        yield return new WaitForSeconds(beginTime2);
        //第二波弹幕
        dir = GetAttackDir();
        angleOri = Vector2.SignedAngle(Vector2.right, dir);
        for (int i = 0; i < frequency1; i++)
        {
            int offset = 0;
            for (int j = 0; j < bulletNumPerFire1; j++)
            {
                offset += j * (j % 2 == 0 ? -1 : 1);
                Quaternion quaternion = Quaternion.Euler(0, 0, angleOri + sumAngle / (bulletNumPerFire1 - 1) * offset);
                CreateBullet(firePoint.position, quaternion);
            }
            yield return new WaitForSeconds(fireInterval);
        }
        //结束动画
        animator.SetTrigger("fireEnd");

        reached = false;
        isSkill = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    Bullet CreateBullet(Vector2 position, Quaternion rotation)
    {
        GameObject go = bulletPool.GetGameObject();
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Init(position, rotation, bulletSpeed, false, bulletDamage, bulletPool);
        return bullet;
    }

    bool inAir = false;
    /// <summary>
    /// 飞天
    /// </summary>
    IEnumerator Skill3()
    {
        reached = true;
        SpriteRenderer[] renderers = GFX.GetComponentsInChildren<SpriteRenderer>(true);
        float t = 0;
        //第一波飞天
        for(int i = 0; i < jumpNumStep1; i++)
        {
            //up动画
            animator.SetTrigger("jump out");
            yield return new WaitForSeconds(beginTime3);
            upVFX.Play(true);
            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = false;
            }

            collider2D.enabled = false;
            inAir = true;

            t = 0;
            while (t < timeUp)
            {
                yield return null;
                t += Time.deltaTime;
                shadow.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * minShadowSize, t / timeUp);
            }

            //在空中
            reached = false;
            speed *= 2;
            yield return new WaitForSeconds(timeOn);
            reached = true;
            speed /= 2;
            //down动画
            //transform.position += (Vector3)GetDirToPlayer();

            t = 0;
            while (t < timeDown)
            {
                yield return null;
                t += Time.deltaTime;
                shadow.transform.localScale = Vector3.Lerp(Vector3.one * minShadowSize, Vector3.one, t / timeDown);
            }

            animator.SetTrigger("jump in");
            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = true;
            }
            yield return new WaitForSeconds(endTime3);
            downVFX.Play(true);

            collider2D.enabled = true;

            if (Vector2.Distance(targetMO.transform.position, foot.transform.position) < areaSkill3)
            {
                targetMO.GetHurt(damageSkill3, Vector2.up);
            }
            inAir = false;
            yield return new WaitForSeconds(intervalPerJump);
        }
        //间隔
        yield return new WaitForSeconds(stepIntervalSkill2);

        //第二次飞天
        for (int i = 0; i < jumpNumStep2; i++)
        {
            //up动画
            animator.SetTrigger("jump out");
            yield return new WaitForSeconds(beginTime3);
            upVFX.Play(true);
            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = false;
            }

            collider2D.enabled = false;
            inAir = true;
            t = 0;
            while (t < timeUp)
            {
                yield return null;
                t += Time.deltaTime;
                shadow.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * minShadowSize, t / timeUp);
            }

            //在空中
            reached = false;
            speed *= 2;
            yield return new WaitForSeconds(timeOn);
            reached = true;
            speed /= 2;
            //down动画
            //transform.position += (Vector3)GetDirToPlayer();

            t = 0;
            while (t < timeDown)
            {
                yield return null;
                t += Time.deltaTime;
                shadow.transform.localScale = Vector3.Lerp(Vector3.one * minShadowSize, Vector3.one, t / timeDown);
            }

            animator.SetTrigger("jump in");
            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = true;
            }
            yield return new WaitForSeconds(endTime3);
            downVFX.Play(true);

            collider2D.enabled = true;

            if (Vector2.Distance(targetMO.transform.position, foot.transform.position) < areaSkill3)
            {
                targetMO.GetHurt(damageSkill3, Vector2.up);
            }
            inAir = false;
            yield return new WaitForSeconds(intervalPerJump);
        }
        //结束动画

        reached = false;
        isSkill = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    /// <summary>
    /// 召唤小怪
    /// </summary>
    IEnumerator Skill4()
    {
        reached = true;
        //动画
        animator.SetTrigger("summon");
        yield return new WaitForSeconds(1.1f);
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = bornPoint[i].position;
            //处理坐标

            //开特效
            callEnemyVFXs[i].transform.position = pos;
            callEnemyVFXs[i].Play(true);
            //实例化敌人
            GameObject enemyGo = Instantiate(enemyPrafabs[i], pos, Quaternion.identity,transform.parent);
            enemyGo.transform.position += enemyGo.transform.position - enemyGo.GetComponent<MoveObject>().foot.transform.position;
            Game.instance.curEnemies.Add(enemyGo.GetComponent<MoveObject>());
            Game.instance.CurEnemyCount++;

            yield return new WaitForSeconds(0.5f);
        }

        //结束
        reached = false;
        isSkill = false;
        Game.instance.InformEnemie(player.GetMoveObject());
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    private void OnDestroy()
    {
        player.InformPossessEvent -= Player_InformPossessEvent;
        player.InformOutPossessEvent -= Player_InformOutPossessEvent;

        PoolManager.instance.LogOutPool(bulletPool);
    }
}
