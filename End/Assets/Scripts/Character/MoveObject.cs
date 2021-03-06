using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MoveObject : MonoBehaviour
{
    public MoveObjectType type;

    [Header("攻击")]
    public GameObject bulletPrefab;
    public float attackRadius = 5f;
    private AttackTimer attackTimer;
    [Tooltip("弹匣容量，默认为0（近战）")]
    public int bulletNum = 0;
    [Tooltip("换弹时间,默认为0（近战）")]
    public float shotReload = 0f;
    [Tooltip("单次攻击间隔")]
    public float attackSpace = 2f;
    [Tooltip("子弹速度")]
    public float bulletSpeed = 5f;
    [Tooltip("子弹伤害")]
    public float bulletDamage = 20f;

    public float shakeTime = 0f;
    public float cameraShakeIntensity = 1f; 
    public float cameraShakeTime = 0.05f; 

    public float FearRadius = 5f;
    public AudioSource audioSource;
    //控制图片整体，控制朝向
    [Header("图片")]
    public Transform GFX;
    public Animator[] animators;
    public Transform foot;
    public Transform attackPoint;
    public GameObject fearTex;
    public Slider HP_Slider;
    public Slider HP_Slider_Bg;
    public Slider Slider_Reload;
    public LineRenderer lineRenderer;
    public GameObject PossessTex;
    public GameObject PossessCircle;
    public float injureAnimDur = 2f;

    [Header("其他")]
    public float MaxHp = 100f;
    public float speed = 10f;
    public float ReloadTime = 2f;

    [Header("玩家")]
    public GameObject bulletPrefab_player;
    public float speed_player = 5f;
    [Tooltip("弹匣容量，默认为0（近战）")]
    public int bulletNum_player = 0;
    [Tooltip("换弹时间,默认为0（近战）")]
    public float shotReload_player = 0f;
    [Tooltip("单次攻击间隔")]
    public float attackSpace_player = 2f;
    [Tooltip("子弹速度")]
    public float bulletSpeed_player = 5f;
    [Tooltip("子弹伤害")]
    public float bulletDamage_player = 20f;

    protected float Hp;

    [HideInInspector]
    public bool isPlayer;
    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    [HideInInspector]
    public Controller controller;
    protected Material material_Body;
    protected Material material_Edge;
    protected float OriScale;
    //子弹池
    protected Pool bulletPool;

    //状态机
    public enum State
    {
        Normal,
        Injure,
        Dead,
        Roll,
        DeadDead
    }
    [Space(20)]
    public State _State;
    private void Reset()
    {
        //rigidbody = gameObject.AddComponent<Rigidbody2D>();
        //collider = gameObject.AddComponent<Collider2D>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        OriScale = transform.localScale.x;

        attackTimer = new AttackTimer(bulletNum, shotReload, attackSpace);
        _State = State.Normal;
        Hp = MaxHp;
        isPlayer = false;

        if (type == MoveObjectType.Dead) return;

        material_Body = new Material(Game.instance.RoleShader);
        material_Edge = new Material(Game.instance.RoleShader);
        SpriteRenderer[] sprites = GFX.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in sprites)
        {
            renderer.material = material_Body;
        }
        SpriteRenderer[] sprites1 = GFX.GetChild(1).GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in sprites1)
        {
            renderer.material = material_Edge;
        }
        Slider_Reload.gameObject.SetActive(false);
        CloseHP();

        if (bulletPrefab)
        {
            bulletPool = PoolManager.instance.RegisterPool(bulletPrefab);
            bulletPool.registor.Add(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DefaultUpdate();
    }
    /// <summary>
    /// 攻击计时、附身检测
    /// </summary>
    protected void DefaultUpdate()
    {
        attackTimer.Update();
        if (_State == State.Roll && DefaultSkill)
        {
            Canying();
        }

        //附身检测代码
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform player = Game.instance.playerController.transform;
        if (type == MoveObjectType.Dead)
        {
            if (Vector2.Distance(transform.position, player.position) < 5)
            {
                if (!isPlayer)
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, player.position);
                    lineRenderer.gameObject.SetActive(true);
                    PossessTex.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E) && !isPlayer)
                {
                    if (Vector2.Distance(transform.position, mousePos) < 1f)
                    {
                        Game.instance.playerController.Possess(this);
                        lineRenderer.gameObject.SetActive(false);
                        PossessTex.SetActive(false);
                        isPlayer = true;
                    }
                }
            }
            else
            {
                lineRenderer.gameObject.SetActive(false);
                PossessTex.SetActive(false);
            }
        }
        else if (_State == State.Dead)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.position);
            lineRenderer.gameObject.SetActive(true);
            PossessTex.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) && !isPlayer)
            {
                if (Vector2.Distance(transform.position, mousePos) < 1f)
                {
                    StopCoroutine(nameof(DeadNoPossess));
                    StartCoroutine(nameof(Possess));
                    PlayerController.instance.Possess(this);
                    _State = State.Injure;
                    lineRenderer.gameObject.SetActive(false);
                    PossessTex.SetActive(false);

                    //canying
                    if (type == MoveObjectType.Living)
                    {
                        RegisterCanying();
                    }
                }
            }
        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
            PossessTex.SetActive(false);
        }
    }
    public virtual bool MouseBtnLeftDown(Vector2 targetPos)
    {
        TurnTowards(targetPos.x < foot.position.x);
        return CallAttack(targetPos);
    }
    public virtual void MouseBtnLeftUp(Vector2 targetPos)
    {

    }
    public virtual void Attack(Vector2 target)
    {
        if (isPlayer)
        {
            ((PlayerController)controller).CameraShakeShot(((Vector2)transform.position - target).normalized * cameraShakeIntensity, cameraShakeTime);
        }
    }
    public virtual void MouseBtnRightDown(Vector2 targetPos)
    {
        Skill(targetPos);
    }
    public virtual void MouseBtnRightUp(Vector2 targetPos)
    {

    }
    protected bool DefaultSkill = true;
    public virtual void Skill(Vector2 target)
    {
        if (_State == State.Roll) return;
        _State = State.Roll;
        DefaultSkill = true;
        collider.enabled = false;
        transform.DOMove((target - rigidbody.position).normalized * 5f + rigidbody.position, 1f);
        PlayAnim("roll");
        SetAnimLayerWeight(0f);

        SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[22]);
        Invoke(nameof(AnimaInjureFinish), 1f);
    }
    /// <summary>
    /// 残影
    /// </summary>
    protected virtual void Canying()
    {
        ShadowPool.instance.SetCanying(GFX.GetChild(1).GetChild(0));
    }
    protected virtual void RegisterCanying()
    {
        ShadowPool.instance.RegisterCanying(GFX.GetChild(1).GetChild(0).gameObject, 10);
    }
    public void MoveUpdate(Vector2 dir, float speedScale)
    {
        if (_State == State.Normal)
        {
            Vector3 detal = (Vector3)dir.normalized * speed * speedScale * Time.deltaTime;
            transform.position += detal;
        }
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="dirXscale"></param>
    /// <param name="animSpeed"></param>
    public void MoveVelocity(Vector2 dirXscale, float animSpeed)
    {
        if (_State == State.Normal)
        {
            rigidbody.velocity = speed * dirXscale;
            SetAnimSpeed(animSpeed);
            SetAnimLayerWeight(Mathf.Floor(animSpeed));
            if (animSpeed > 0 && isPlayer)
            {               
                //SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[23]);
            }
        }
    }
    public void MoveVelocity(Vector2 dirXscale, float animSpeed, bool isLeft)
    {
        if (_State == State.Normal)
        {
            rigidbody.velocity = speed * dirXscale;
            SetAnimSpeed(animSpeed);
            SetAnimLayerWeight(Mathf.Floor(animSpeed));
            if (isPlayer && animSpeed > 0 && type == MoveObjectType.Living && (dirXscale.x < 0) == isLeft)
            {
                Game.instance.weiqi.SetWeiqiPosition(foot.position, dirXscale.x < 0);
            }
            if (animSpeed > 0 && isPlayer)
            {
                //SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[23]);
            }
        }
    }
    public virtual void TurnTowards(bool isleft)
    {
        if (_State == State.Normal)
        {
            GFX.localScale = new Vector3(isleft ? 1 : -1, 1, 1);
        }
    }
    bool playerInjureCDing = false;
    bool injureCDing = false;
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="value"></param>
    /// <param name="force"></param>
    public virtual void GetHurt(float value, Vector2 force, bool forceAutoNormalize = true)
    {
        if (forceAutoNormalize)
            force = force.normalized;
        if (_State == State.Dead || _State == State.DeadDead || _State == State.Roll || playerInjureCDing || injureCDing) return;
        Hp -= value;
        injureCDing = true;
        Invoke(nameof(InjureCDFinish), 0.1f);
        if (Hp > 0)
        {
            PlayAnim("injure");
            if (isPlayer)
            {
                playerInjureCDing = true;
                collider.enabled = false;
                StartCoroutine(nameof(PlayerInjured));
                Invoke(nameof(AnimaInjureFinish), injureAnimDur);
                ((PlayerController)controller).GetHurtEffect(force);
            }
            else
            {
                SetAnimLayerWeight(0f);
                StartCoroutine(nameof(EnemyInjured));
                rigidbody.AddForce(force*300);
            }
            
        }
        else
        {
            PlayAnim("dead");
            SetAnimLayerWeight(0f);

            _State = State.Dead;
            rigidbody.AddForce(force * 1000);

            //SoundManager.instance.PlaySoundClipRandom(25, 29, SoundManager.instance.combatSound);
            audioSource.PlayOneShot(SoundManager.instance.GetSoundClipRandom(25, 29, SoundManager.instance.combatSound));
            if (isPlayer)
            {
                //Player dead;
                ((PlayerController)controller).MoveObjectDead();
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(nameof(DeadNoPossess));
                StopCoroutine(nameof(EnemyInjured));

                _State = State.Dead;
                
                Game.instance.DeleteEnemyMO(this);
                Destroy(controller);
                collider.enabled = false;
                rigidbody.velocity = Vector2.zero;
                fearTex.SetActive(false);

                //controller.enabled = false;
                material_Body.SetVector("_Color1", new Vector4(0.6792453f, 0.6792453f, 0.6792453f, 1));
                material_Edge.SetVector("_Color1", new Vector4(0, 0.9441266f, 1,1));
                material_Body.SetFloat("_Shine", 0f);

            }
        }
        CancelInvoke(nameof(CloseHP));
        Invoke(nameof(CloseHP), 2f);
        ShowHP();
        HP_Slider.value = Hp / MaxHp;
        StartCoroutine(SliderValue(0.7f, HP_Slider_Bg, Hp / MaxHp));
    }
    void InjureCDFinish()
    {
        injureCDing = false;
    }
    IEnumerator SliderValue(float time,Slider slider,float targetValue)
    {
        float t = 0;
        while (t < time)
        {
            slider.value = Mathf.Lerp(slider.value, targetValue, t / time);
            t += Time.deltaTime;
            yield return null;
        }
    }
    protected void ShowHP()
    {
        HP_Slider.gameObject.SetActive(true);
        HP_Slider_Bg.gameObject.SetActive(true);
    }
    protected void CloseHP()
    {
        HP_Slider.gameObject.SetActive(false);
        HP_Slider_Bg.gameObject.SetActive(false);
    }
    public void AnimaInjureFinish()
    {
        collider.enabled = true;
        playerInjureCDing = false;
        _State = State.Normal;
    }
    public void AnimaDeadFinish()
    {

    }
    protected IEnumerator DeadNoPossess()
    {
        yield return new WaitForSeconds(5f);
        DeadDead();
    }
    protected IEnumerator Possess()
    {
        yield return new WaitForSeconds(2f);
        material_Body.SetVector("_Color1", Vector4.one);
        material_Edge.SetVector("_Color1", Vector4.one);
        //冲击波
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.useLayerMask = true;
        filter2D.SetLayerMask(LayerMask.GetMask("AttackPhysics"));
        filter2D.useTriggers = true;
        if (Physics2D.OverlapCircle(transform.position, 3f, filter2D, colliders) > 0)
        {
            foreach(var bulletCol in colliders)
            {
                Bullet bullet;
                if(bulletCol.TryGetComponent<Bullet>(out bullet))
                {
                    bullet.DestroyBulletSelf();
                }
            }
        }
    }
    public void PlayerLeaveThisBody()
    {
        PossessCircle.SetActive(false);
        PlayAnim("dead");
        MoveVelocity(Vector2.zero, 0f);
        Invoke(nameof(DeadDead), 3f);
    }
    protected void DeadDead()
    {
        _State = State.DeadDead;
        collider.enabled = false;
        if(TryGetComponent(out EnemyController enemyController))
        {
            Destroy(enemyController);
        }
        material_Body.SetVector("_Color1", new Vector4(0.6792453f, 0.6792453f, 0.6792453f, 1));
        material_Edge.SetVector("_Color1", new Vector4(0.6792453f, 0.6792453f, 0.6792453f, 1));
        //Destroy(gameObject);
        Destroy(collider);
        Destroy(rigidbody);
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
        _State = State.Injure;
        material_Body.SetFloat("_Shine", 0.5f);
        yield return new WaitForSeconds(0.1f);
        material_Body.SetFloat("_Shine", 0f);
        yield return new WaitForSeconds(0.1f);
        if (_State==State.Injure || _State==State.Roll)
        {
            _State = State.Normal;
        }
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
                PossessCircle.SetActive(true);
            }
            collider.enabled = true;
            attackTimer.SetAttackTimer(bulletNum_player, shotReload_player, attackSpace_player);
            speed = speed_player;
            bulletSpeed = bulletSpeed_player;
            bulletDamage = bulletDamage_player;

            if (bulletPrefab_player)
            {
                PoolManager.instance.LogOutPool(bulletPool);
                bulletPool = PoolManager.instance.RegisterPool(bulletPrefab_player);
                bulletPool.registor.Add(gameObject);
            }
            SetPlayer();
        }
        _State = State.Normal;

    }
    /// <summary>
    /// 附身时要额外干啥写到这里面来
    /// </summary>
    public virtual void SetPlayer()
    {

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
    public void PlayAnim(string anim,int i)
    {
        foreach (Animator animator in animators)
        {
            animator.SetInteger(anim,i);
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
        int check = attackTimer.AttackCheck();

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
                if (isPlayer)
                {
                    StartCoroutine(nameof(Reload));
                    ReloadSound();
                }
                break;
        }
        return false;
    }
    protected IEnumerator Reload()
    {
        Slider_Reload.gameObject.SetActive(true);
        StartCoroutine(SliderValue(shotReload, Slider_Reload, 1));
        yield return new WaitForSeconds(shotReload);
        audioSource.Stop();
        Slider_Reload.value = 0;
        Slider_Reload.gameObject.SetActive(false);
    }
    protected virtual void ReloadSound()
    {

    }
    /// <summary>
    /// 发射一颗子弹
    /// </summary>
    /// <param name="position"></param>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public Bullet CreateBullet(Vector2 position, Quaternion rotation)
    {
        GameObject go = bulletPool.GetGameObject();
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Init(position, rotation, bulletSpeed, isPlayer, bulletDamage, bulletPool);
        return bullet;
    }
    public float GetHP()
    {
        return Hp;
    }

    public virtual void CollideWhenRush(string collisionTag)
    {
        
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
        if (bulletPool)
        {
            PoolManager.instance.LogOutPool(bulletPool);
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

