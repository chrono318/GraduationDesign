using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SeagullsMO : MoveObject
{
    public GameObject clock;
    public GameObject dizzy;
    public float hurtRadius = 5f;
    public float hurtValue = 100f;
    public float preBoomRadius = 3f;
    private bool preBoom = false;
    public float rushRadius = 20f;
    public float rushSpeed = 20f;
    bool isWaitForBtn = false;
    bool isRush = false;
    public override bool MouseBtnLeftDown(Vector2 targetPos)
    {
        if (!CallAttack(targetPos)) return false;
        if (isPlayer)
        {
            if (isWaitForBtn)//第二次
            {
                CancelInvoke(nameof(Boom));
                Boom();
                return true;
            }
            else                       //第一次
            {
                isWaitForBtn = true;
                clock.SetActive(true);
                Invoke(nameof(Boom), 5f);
            }
        }
        else
        {
            if (isRush) return true;
            isRush = true;
            PlayAnim("animState", 1);
            StartCoroutine(Rush((targetPos - rigidbody.position).normalized));
        }
        return false;
    }
    public override void Attack(Vector2 target)
    {
        
    }
    private void Update()
    {
        DefaultUpdate();

        if (clock.activeSelf)
        {
            Clock();
        }
        if (isPlayer) return;
        MoveObject player = Game.instance.playerController.GetMoveObject();
        if (player == null) return;
        if (player.type==MoveObjectType.Dead) return;
        if (Vector2.Distance(player.transform.position, rigidbody.position) < preBoomRadius)
        {
            if (!preBoom) {
                preBoom = true;
                PlayAnim("animState", 4);
                return;
            }
            //Boom
            Boom();
        }
        
    }
    void Boom()
    {
        PlayAnim("animState", 2);
        clock.SetActive(false);
        if (isPlayer)
        {
            foreach (MoveObject mo in Game.instance.curEnemies)
            {
                if (mo != null)
                {
                    if (mo.type == MoveObjectType.Dead) continue;
                    if (Vector2.Distance(mo.transform.position, rigidbody.position)<hurtRadius)
                    {
                        mo.GetHurt(hurtValue, ((Vector2)mo.transform.position - rigidbody.position).normalized*100);
                    }
                }
            }
            ((PlayerController)controller).MoveObjectDead();
        }
        else
        {
            MoveObject player = Game.instance.playerController.GetMoveObject();
            if (Vector2.Distance(player.transform.position, rigidbody.position) < hurtRadius && player._State != State.Roll && player.type==MoveObjectType.Living)
            {
                player.GetHurt(hurtValue, ((Vector2)player.transform.position - rigidbody.position).normalized*100);
            }
            Game.instance.CheckIfPass();
            Game.instance.DeleteEnemyMO(this);
        }
        Invoke(nameof(DestroySelf), 1f);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void Clock()
    {
        clock.transform.Find("CD").GetComponent<Image>().fillAmount -= -1.0f / 5f * Time.deltaTime;
        //指针旋转
        clock.transform.Find("Pointer").gameObject.transform.rotation = Quaternion.Euler(0, 0, 90 + clock.transform.Find("CD").GetComponent<Image>().fillAmount * -360);
    }
    IEnumerator Rush(Vector2 dir)
    {
        Vector2 oriPos = rigidbody.position;
        float dis = 0;
        MoveVelocity(dir * rushSpeed,1);
        while (dis < rushRadius)
        {
            dis = (rigidbody.position - oriPos).magnitude;
            yield return 0;
        }
        //休息
        _State = State.Injure;
        PlayAnim("animState", 0);
        Invoke(nameof(AnimaInjureFinish), 3f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer==LayerMask.NameToLayer("Obstacles") && isRush)
        {
            //眩晕
            PlayAnim("animState", 3);
            _State = State.Injure;
            Invoke(nameof(AnimaInjureFinish), 3f);
        }
    }
}
