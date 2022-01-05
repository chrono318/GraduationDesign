using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Vigilance : StateTemplate<Enemy>
{
    float hurtTime = 0.7f;
    float m_time = 0f;
    public Default_Vigilance(int id,Enemy o):base(id,o)
    {
            
    }
    public override void OnEnter()
    {
        m_time = 0;
        owner.enemyAI.speed_anim = 0f;
        owner.animator.SetFloat("speed", 0);
    }

    public override void OnExit()
    {
        if (machine.isplayer)
        {
            machine.player.canMove = true;
        }
    }

    public override void OnStay()
    {
        m_time += Time.deltaTime;
        if (m_time >= hurtTime)
        {
            if (owner.hasTarget)
            {
                machine.TranslateToState(2);
            }
            else
            {
                machine.TranslateToState(1);
            }
        }
    }
}
