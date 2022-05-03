using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBoss : Boss
{
    public float attackCD = 10f;
    public float attackDamage = 10f;
    private float _t = 0f;
    enum BossState
    {
        idle,
        persuit,  //׷��
        attack,
    }
    BossState _state = BossState.idle;
    [Header("����")]
    public float firstTime = 5f;
    public float skillCD = 10f;
    private int lastSkillIndex = 0;
    public Transform firePoint;

    [Header("����-���")]
    public float beginTime0 = 1f;
    public float endTime0 = 1f;
    public float rushDistance = 10f;
    public float rushDuration = 1f;
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
    [Header("����-��Ļ")]
    public float beginTime2 = 1f;
    public float endTime2 = 1f;
    public GameObject bullet;
    //�ӵ���
    private Pool bulletPool;
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
    [Header("�ٻ�С��")]
    public float interval = 10f;//���
    public List<GameObject> enemyPrafabs;
    public List<Transform> bornPoint;

    // Start is called before the first frame update
    void Start()
    {
        _state = BossState.idle;
        player.InformPossessEvent += Player_InformPossessEvent;
        player.InformOutPossessEvent += Player_InformOutPossessEvent;
        InvokeRepeating(nameof(BurePerTime), 0, 1f);

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        if (path == null || reached)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        Vector2 direction = (path.vectorPath[currentWaypoint] - foot.position);

        transform.position += (Vector3)direction * Time.deltaTime * speed * speedScaleSkill1;

        float distance = direction.magnitude;

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            if (path.vectorPath.Count > currentWaypoint)
            {
                float dirForAnim = path.vectorPath[currentWaypoint].x - path.vectorPath[currentWaypoint - 1].x;
                GFX.localScale = new Vector3(dirForAnim > 0 ? -1 : 1, 1, 1);
            }
        }
    }

    bool inAir = false;
    /// <summary>
    /// ����Ч����������Ѫ
    /// </summary>
    void BurePerTime()
    {
        if (!targetMO || inAir) return;
        Vector2 dir = GetDirToPlayer();
        if (dir.magnitude <= attackDis)
        {
            targetMO.GetHurt(attackDamage, dir);
        }
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
        lastSkillIndex = index;
    }
    void StartSkill(int index)
    {
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
        reached = true;
        float t = 0;
        while (t < beginTime0)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, (t / beginTime0));
            t += Time.deltaTime;
            yield return 0;
        }

        t = 0;
        Vector2 dir = GetDirToPlayer();
        float _speed = rushDistance / rushDuration;
        
        while (t < rushDuration)
        {
            transform.position += (Vector3)dir.normalized * _speed * Time.deltaTime;
            t += Time.deltaTime;
            yield return 0;
        }

        t = 0;
        while (t < endTime0)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, (t / endTime0));
            t += Time.deltaTime;
            yield return 0;
        }
        reached = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    /// <summary>
    /// ����
    /// </summary>
    IEnumerator Skill1()
    {
        speed *= speedScaleSkill1;
        float t = 0;
        while (t < beginTime1)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.green, (t / beginTime1));
            t += Time.deltaTime;
            yield return 0;
        }
        //��һ�׶�
        Vector2 dir = GetAttackDir();
        LineRenderer line0 = gameObject.AddComponent<LineRenderer>();
        t = 0;
        bool bo = false;
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

        //���ʱ��
        t = 0;
        while (t < gapDuration)
        {
            t += Time.deltaTime;
            yield return 0;
        }
        //�ڶ��׶�
        t = 0;
        line0.enabled = true;
        GameObject go1 = new GameObject();
        GameObject go2 = new GameObject();
        LineRenderer line1 = go1.AddComponent<LineRenderer>();
        LineRenderer line2 = go2.AddComponent<LineRenderer>();
        dir = GetAttackDir();
        Vector2 dir1 = Quaternion.Euler(0, 0, 45f) * dir;
        Vector2 dir2 = Quaternion.Euler(0, 0, -45f) * dir;
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
        Destroy(line0);
        Destroy(go1);
        Destroy(go2);
        //��������
        t = 0;
        while (t < endTime0)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.white, (t / endTime0));
            t += Time.deltaTime;
            yield return 0;
        }
        speed /= speedScaleSkill1;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    /// <summary>
    /// ��Ļ
    /// </summary>
    /// <returns></returns>
    IEnumerator Skill2()
    {
        reached = true;
        float t = 0;
        while (t < beginTime2)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.blue, (t / beginTime2));
            t += Time.deltaTime;
            yield return 0;
        }
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

        yield return new WaitForSeconds(stepIntervalSkill2);

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
        t = 0;
        while (t < endTime2)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.blue, Color.white, (t / endTime2));
            t += Time.deltaTime;
            yield return 0;
        }
        reached = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    Bullet CreateBullet(Vector2 position, Quaternion rotation)
    {
        GameObject go = bulletPool.GetGameObject();
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Init(position, rotation, speed, false, bulletPool);
        return bullet;
    }
    /// <summary>
    /// ����
    /// </summary>
    IEnumerator Skill3()
    {
        reached = true;
        float t = 0;
        while (t < beginTime3)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.black, (t / beginTime2));
            t += Time.deltaTime;
            yield return 0;
        }
        //��һ������
        for(int i = 0; i < jumpNumStep1; i++)
        {
            //up����
            yield return new WaitForSeconds(timeUp);
            GFX.localScale = Vector3.one * 0.2f;
            inAir = true;
            yield return new WaitForSeconds(timeOn);
            //down����
            transform.position += (Vector3)GetDirToPlayer();
            yield return new WaitForSeconds(timeDown);
            GFX.localScale = Vector3.one;
            if (Vector2.Distance(targetMO.transform.position, foot.transform.position) < areaSkill3)
            {
                targetMO.GetHurt(damageSkill3, Vector2.up);
            }
            inAir = false;
            yield return new WaitForSeconds(intervalPerJump);
        }
        //���
        GFX.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(stepIntervalSkill2);
        GFX.GetComponent<SpriteRenderer>().color = Color.black;

        //�ڶ��η���
        for (int i = 0; i < jumpNumStep2; i++)
        {
            //up����
            yield return new WaitForSeconds(timeUp);
            GFX.localScale = Vector3.one * 0.2f;
            inAir = true;
            yield return new WaitForSeconds(timeOn);
            //down����
            transform.position += (Vector3)GetDirToPlayer();
            yield return new WaitForSeconds(timeDown);
            GFX.localScale = Vector3.one;
            if (Vector2.Distance(targetMO.transform.position, foot.transform.position) < areaSkill3)
            {
                targetMO.GetHurt(damageSkill3, Vector2.up);
            }
            inAir = false;
            yield return new WaitForSeconds(intervalPerJump);
        }
        //��������
        t = 0;
        while (t < endTime2)
        {
            GFX.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, Color.white, (t / endTime2));
            t += Time.deltaTime;
            yield return 0;
        }
        reached = false;
        Invoke(nameof(ContinueSkillTiming), skillCD);
    }
    private void OnDestroy()
    {
        player.InformPossessEvent -= Player_InformPossessEvent;
        player.InformOutPossessEvent -= Player_InformOutPossessEvent;

        PoolManager.instance.LogOutPool(bulletPool);
    }
}
