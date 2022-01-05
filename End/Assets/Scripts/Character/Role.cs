using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    public RoleType roleType;
    public float speed = 10f;
    private float HP = 100f;
    public HPDefault hpUI;
    private float San = 100f;

    public float deadAnimaDur = 1.5f;
    //状态机
    [HideInInspector]
    public StateMachine machine;

    public Animator animator;

    //用于受伤变白的spriteRender
    protected SpriteRenderer[] spriteRenderers;
    protected Material material;
    //public Shader shader;

    // Start is called before the first frame update
    public void Init()
    {
        material = new Material(Game.instance.RoleShader);

        //存颜色
        if (spriteRenderers == null)
        {
            spriteRenderers = transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sharedMaterial = material;
            }
        }
        hpUI = transform.GetComponentInChildren<HPDefault>();

        Attack_Item[] attack_Items = transform.GetComponentsInChildren<Attack_Item>();
        foreach(Attack_Item item in attack_Items)
        {
            item.animator = animator;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void GetHurt(float value,RoleType attackType,Vector2 force)
    {
        deadDir = force;
        if (attackType == RoleType.Physics)
        {
            AddHP(value * -1);
        }
        else
        {
            AddSan(value * -1);
        }
        animator.SetTrigger("injure");
    }
    //dead
    protected Vector2 deadDir;
    public virtual void Dead()
    {

        animator.SetTrigger("dead");
        

        Invoke(nameof(Anima_deadFinished), deadAnimaDur);
    }
    public virtual void AddHP(float value)
    {
        HP += value;
        if (HP <= 0)
        {
            Dead();
        }
        if (hpUI)
        {
            hpUI.SetValue(HP / 100);
        }
        else
        {
            print("No hpUI   name:"+this);
        }
    }
    public virtual void AddSan(float value)
    {
        San += value;
    }

    public IEnumerator EnemyInjured()
    {
        material.SetFloat("_Shine", 0.5f);
        yield return new WaitForSeconds(0.1f);
        material.SetFloat("_Shine", 0f);
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
    public void Anima_deadFinished()
    {
        //GetComponent<Collider2D>().enabled = false;
        //Destroy(gameObject);
    }
}
