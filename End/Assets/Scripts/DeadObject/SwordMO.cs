using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMO : MoveObject
{
    bool isLeft = true;
    public override void Attack(Vector2 target)
    {
        isLeft = target.x < transform.position.x;
        PlayAnim("attack");
        Invoke(nameof(checkAttack), 0.66f);
    }
    void checkAttack()
    {
        CameraControl.instance.CameraShakeShot(new Vector2(isLeft ? -1 : 1, -1), 0.1f);
        if (isPlayer)
        {
            List<MoveObject> enemies = Game.instance.curEnemies;
            for(int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    if (AttackDis(enemies[i].foot.position, foot.position, isLeft))
                    {
                        enemies[i].GetHurt(51, (enemies[i].foot.position - foot.position).normalized);
                    }
                }
            }
        }
        else
        {
            MoveObject player = Game.instance.playerController.GetMoveObject();
            if (AttackDis(player.foot.position, foot.position, isLeft) && player._State != State.Roll)
            {
                player.GetHurt(51, (player.foot.position - foot.position).normalized);
            }
        }
    }
    bool AttackDis(Vector2 tar, Vector2 own, bool isLeft)
    {
        float detalX = (tar.x - own.x) * (isLeft ? -1 : 1);
        if (detalX <= attackRadius && detalX>0)
        {
            if (Mathf.Abs(tar.y - own.y) < attackRadius / 2)
            {
                return true;
            }
        }
        return false;
    }
    public override void Skill(Vector2 target)
    {
        
    }
}
