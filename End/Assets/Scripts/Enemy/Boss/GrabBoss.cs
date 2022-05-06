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
        persuit,  //׷��
        attack,
    }
    BossState _state = BossState.idle;
    [Header("��ͨ����")]
    private bool isAttacking = false;
    public float attackCD = 10f;
    public float attackDamage = 10f;
    [Header("����")]
    public float firstTime = 5f;
    public float skillCD = 10f;
    private int lastSkillIndex = 0;
    private bool isSkill = false;
    public Transform firePoint;

    [Header("����-���")]
    public float beginTime0 = 1f;
    public float endTime0 = 1f;
    public float rushDistance = 10f;
    public float rushDuration = 1f;
    public float rushAttackWidth = 3f;
    [Header("����-����")]
    public float beginTime1 = 1f;
    public float endTime1 = 1f;
    public float damagePerSecond = 30f;
    public float widthSkill1 = 1f;
    public float lengthSkill1 = 10f;
    public float speedScaleSkill1 = 0.2f;
    public float step1Duration = 5f;
    public float gapDuration = 1f;
    public float step2Duration = 5f;
    public LineRenderer line0;
    public LineRenderer line1;
    public LineRenderer line2;
    [Header("����-��Ļ")]
    public float beginTime2 = 1f;
    public float endTime2 = 1f;
    public GameObject bullet;
    //�ӵ���
    private Pool bulletPool;
    public float bulletSpeed = 5f;
    public float sumAngle = 60f;
    public float fireInterval = 0.3f;
    public float stepIntervalSkill2 = 2f;
    public int frequency = 5;
    public int bulletNumPerFire = 5;
    public int frequency1 = 7;
    public int bulletNumPerFire1 = 7;
    [Header("����-����")]
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
    [Header("�ٻ�С��")]
    public float interval = 10f;//���
    public List<GameObject> enemyPrafabs;
    public List<Transform> bornPoint;

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

        StopAllCoroutines();
        reached = true;
        isAttacking = false;
        isSkill = false;
        animator.Play("idle");

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
        if (!isSkill && !isAttacking && targetMO)
        {
            Vector2 dir = GetDirToPlayer();
            if (dir.magnitude <= attackDis)
            {
                StartCoroutine(nameof(Attack));
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
    /// ��ͨ����
    /// </summary>
    IEnumerator Attack()
    {
        animator.SetTrigger("attack");
        isAttacking = true;
        yield return new WaitForSeconds(1f); //���������1s��ӻ�
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
        return targetMO.transform.position - firePoint.position;
    }

    void StartSkillTiming()
    {
        int index = Random.Range(0, 4);
        StartSkill(index);
    }
    void ContinueSkillTiming()
    {
        int index = Random.Range(1, 4);
        index += lastSkillIndex;
        index %= 4;
        StartSkill(index);
    }
    void StartSkill(int index)
    {
        isSkill = true;
        lastSkillIndex = index;
        StopCoroutine(nameof(Attack));
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
        }
    }
    /// <summary>
    /// ��ײ
    /// </summary>
    IEnumerator Skill0()
    {
        //׼��
        reached = true;
        animator.SetTrigger("rushPre");
        yield return new WaitForSeconds(beginTime0);

        //��ײ
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
                targetMO.GetHurt(attackDamage, GetDirToPlayer() * 3 ,false);
            }
            yield return 0;
        }

        //��������
        animator.SetTrigger("rushEnd");
        yield return new WaitForSeconds(endTime0);

        reached = false;
        isSkill = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    /// <summary>
    /// ����
    /// </summary>
    IEnumerator Skill1()
    {
        speedScale = speedScaleSkill1;
        animator.SetTrigger("laserPre");
        yield return new WaitForSeconds(beginTime1);
        //��һ�׶�
        Vector2 dir = GetAttackDir();
        line0.enabled = true;
        line0.transform.GetChild(0).gameObject.SetActive(true);
        line0.transform.GetChild(1).gameObject.SetActive(true);
        line0.transform.GetChild(0).position = (Vector2)firePoint.position;
        line0.transform.GetChild(1).position = (Vector2)firePoint.position + dir.normalized * lengthSkill1;

        line0.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir);
        line0.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir);

        float t = 0;
        bool bo = false;
        animator.SetTrigger("laser");
        while (t < step1Duration)
        {
            line0.SetPosition(0, (Vector2)firePoint.position);
            line0.SetPosition(1, (Vector2)firePoint.position + dir.normalized * lengthSkill1);
            if ((GetAttackDir()-Vector2.Dot(GetAttackDir(),dir.normalized)* dir.normalized).magnitude <= widthSkill1 / 2)
            {
                if (bo)   //��һ֡���ڷ�Χ�ڣ������˺�
                {
                    targetMO.GetHurt(damagePerSecond * Time.deltaTime, dir);
                }
                else  //���ڵĻ�
                {
                    bo = true;
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


        //���ʱ��
        animator.SetTrigger("laserPre");
        yield return new WaitForSeconds(gapDuration);
        //�ڶ��׶�
        t = 0;
        animator.SetTrigger("laser");
        line0.enabled = true;
        line1.enabled = true;
        line2.enabled = true;
        line0.transform.GetChild(0).gameObject.SetActive(true);
        line0.transform.GetChild(1).gameObject.SetActive(true);
        line1.transform.GetChild(0).gameObject.SetActive(true);
        line1.transform.GetChild(1).gameObject.SetActive(true);
        line2.transform.GetChild(0).gameObject.SetActive(true);
        line2.transform.GetChild(1).gameObject.SetActive(true);
        line0.transform.GetChild(0).position = (Vector2)firePoint.position;
        line1.transform.GetChild(0).position = (Vector2)firePoint.position;
        line2.transform.GetChild(0).position = (Vector2)firePoint.position;


        dir = GetAttackDir();
        Vector2 dir1 = Quaternion.Euler(0, 0, 45f) * dir;
        Vector2 dir2 = Quaternion.Euler(0, 0, -45f) * dir;
        line0.transform.GetChild(1).position = (Vector2)firePoint.position + dir.normalized * lengthSkill1;
        line1.transform.GetChild(1).position = (Vector2)firePoint.position + dir1.normalized * lengthSkill1;
        line2.transform.GetChild(1).position = (Vector2)firePoint.position + dir2.normalized * lengthSkill1;

        line0.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir);
        line0.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir);
        line1.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir1);
        line1.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir1);
        line2.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, dir2);
        line2.transform.GetChild(1).rotation = Quaternion.FromToRotation(Vector3.right, dir2);

        bo = false;
        while (t < step2Duration)
        {
            line0.SetPosition(0, (Vector2)firePoint.position);
            line0.SetPosition(1, (Vector2)firePoint.position + dir.normalized * lengthSkill1);

            line1.SetPosition(0, (Vector2)firePoint.position);
            line1.SetPosition(1, (Vector2)firePoint.position + dir1.normalized * lengthSkill1);

            line2.SetPosition(0, (Vector2)firePoint.position);
            line2.SetPosition(1, (Vector2)firePoint.position + dir2.normalized * lengthSkill1);

            if ((GetAttackDir() - Vector2.Dot(GetAttackDir(), dir.normalized) * dir.normalized).magnitude <= widthSkill1 / 2
                || (GetAttackDir() - Vector2.Dot(GetAttackDir(), dir1.normalized) * dir1.normalized).magnitude <= widthSkill1 / 2
                || (GetAttackDir() - Vector2.Dot(GetAttackDir(), dir2.normalized) * dir2.normalized).magnitude <= widthSkill1 / 2)
            {
                if (bo)   //��һ֡���ڷ�Χ�ڣ������˺�
                {
                    targetMO.GetHurt(damagePerSecond * Time.deltaTime, dir);
                }
                else  //���ڵĻ�
                {
                    bo = true;
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
        //��������
        animator.SetTrigger("laserEnd");
        reached = true;
        yield return new WaitForSeconds(endTime1);
        reached = false;

        speedScale = 1f;
        isSkill = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    /// <summary>
    /// ��Ļ
    /// </summary>
    /// <returns></returns>
    IEnumerator Skill2()
    {
        reached = true;
        animator.SetTrigger("firePre");
        yield return new WaitForSeconds(beginTime2);
        //��һ����Ļ
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
        //�ڶ�����Ļ
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
        //��������
        animator.SetTrigger("fireEnd");

        reached = false;
        isSkill = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    Bullet CreateBullet(Vector2 position, Quaternion rotation)
    {
        GameObject go = bulletPool.GetGameObject();
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Init(position, rotation, bulletSpeed, false, bulletPool);
        return bullet;
    }
    /// <summary>
    /// ����
    /// </summary>
    IEnumerator Skill3()
    {
        reached = true;
        SpriteRenderer[] renderers = GFX.GetComponentsInChildren<SpriteRenderer>(true);
        //��һ������
        for(int i = 0; i < jumpNumStep1; i++)
        {
            //up����
            animator.SetTrigger("jump out");
            yield return new WaitForSeconds(timeUp);
            foreach(SpriteRenderer r in renderers)
            {
                r.enabled = false;
            }
            shadow.enabled = false;
            collider2D.enabled = false;
            yield return new WaitForSeconds(timeOn);
            //down����
            transform.position += (Vector3)GetDirToPlayer();
            animator.SetTrigger("jump in");
            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = true;
            }
            shadow.enabled = true;
            yield return new WaitForSeconds(timeDown);
            collider2D.enabled = true;
            if (Vector2.Distance(targetMO.transform.position, foot.transform.position) < areaSkill3)
            {
                targetMO.GetHurt(damageSkill3, Vector2.up);
            }
            yield return new WaitForSeconds(intervalPerJump);
        }
        //���
        yield return new WaitForSeconds(stepIntervalSkill2);

        //�ڶ��η���
        for (int i = 0; i < jumpNumStep2; i++)
        {
            //up����
            animator.SetTrigger("jump out");
            yield return new WaitForSeconds(timeUp);
            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = false;
            }
            shadow.enabled = false;
            collider2D.enabled = false;
            yield return new WaitForSeconds(timeOn);
            //down����
            transform.position += (Vector3)GetDirToPlayer();
            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = true;
            }
            shadow.enabled = true;
            animator.SetTrigger("jump in");
            yield return new WaitForSeconds(timeDown);
            collider2D.enabled = true;
            if (Vector2.Distance(targetMO.transform.position, foot.transform.position) < areaSkill3)
            {
                targetMO.GetHurt(damageSkill3, Vector2.up);
            }
            yield return new WaitForSeconds(intervalPerJump);
        }
        //��������

        reached = false;
        isSkill = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    private void OnDestroy()
    {
        player.InformPossessEvent -= Player_InformPossessEvent;
        player.InformOutPossessEvent -= Player_InformOutPossessEvent;

        PoolManager.instance.LogOutPool(bulletPool);
    }
}
