using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMO : MoveObject
{
    public ParticleSystem Left;
    public ParticleSystem Right;
    public float value = 30f;
    public Vector2 Start_Duration;
    bool isLeft = true;
    public override void Attack(Vector2 target)
    {
        PlayAnim("attack");
        isLeft = (target.x - foot.position.x) < 0;
        if (isLeft)
        {
            Left.Play();
            //Left.GetComponent<Attack_Item>().CheckAttack();
        }
        else
        {
            Right.Play();
            //Right.GetComponent<Attack_Item>().CheckAttack();
        }
        Invoke(nameof(checkAttack), shakeTime);
    }
    void checkAttack()
    {
        if (isPlayer)
        {
            foreach (MoveObject mo in Game.instance.curEnemies)
            {
                if (mo != null)
                {
                    if (AttackDis(mo.foot.position, foot.position,isLeft))
                    {
                        mo.GetHurt(value, (mo.foot.position - foot.position).normalized);
                    }
                }
            }
        }
        else
        {
            MoveObject player = Game.instance.playerController.GetMoveObject();
            if (AttackDis(player.foot.position, foot.position,isLeft) && player._State!=State.Roll)
            {
                player.GetHurt(value, (player.foot.position - foot.position).normalized);
            }
        }
    }
    bool AttackDis(Vector2 tar,Vector2 own,bool isLeft)
    {
        float detalX = (tar.x - own.x) * (isLeft ? -1 : 1);
        if (detalX <= attackRadius && detalX > 0)
        {
            if (Mathf.Abs(tar.y - own.y) < attackRadius / 2)
            {
                return true;
            }
        }
        return false;
    }
}
