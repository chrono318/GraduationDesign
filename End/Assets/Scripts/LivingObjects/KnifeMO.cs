using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMO : MoveObject
{
    public ParticleSystem Left;
    public ParticleSystem Right;
    public float value = 30f;
    public Vector2 Start_Duration;
    public override void Attack(Vector2 target)
    {
        PlayAnim("attack");
        bool isLeft = (target.x - foot.position.x) < 0;
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
        checkAttack(isLeft);
    }
    void checkAttack(bool left)
    {
        if (isPlayer)
        {
            foreach (MoveObject mo in Game.instance.curEnemies)
            {
                if (mo != null)
                {
                    if (AttackDis(mo.foot.position, foot.position,left))
                    {
                        mo.GetHurt(value, (mo.foot.position - foot.position).normalized);
                    }
                }
            }
        }
        else
        {
            MoveObject player = Game.instance.playerController.GetMoveObject();
            if (AttackDis(player.foot.position, foot.position,left) && player._State!=State.Roll)
            {
                player.GetHurt(value, (player.foot.position - foot.position).normalized);
            }
        }
    }
    bool AttackDis(Vector2 tar,Vector2 own,bool isLeft)
    {
        if (tar.x - own.x <= attackRadius)
        {
            if (Mathf.Abs(tar.y - own.y) < attackRadius / 2)
            {
                return true;
            }
        }
        return false;
    }
}
