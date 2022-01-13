using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_PatrolState : StateTemplate<Enemy>
{
    public List<Transform> PatrolPoints;
    private int index = 0;
    private float m_time = 0f;
    private bool LookingAround = false;
    public Default_PatrolState(int id, Enemy o) : base(id, o)
    {
        
    }
    public override void OnEnter()
    {
        base.OnEnter();
        owner.enemyAI.speed_anim = 0.5f;

        if (PatrolPoints.Count == 0) return;
        owner.SetAITarget(PatrolPoints[index]);
    }
    public override void OnStay()
    {
        if (PatrolPoints.Count == 0) return;
        if (LookingAround)
        {
            m_time += Time.deltaTime;

            if (m_time >= 2f)
            {
                m_time = 0f;
                LookingAround = false;
                GoToNextPoint();
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public void OnReach()
    {
        //owner.animator.SetTrigger("lookAround");
        LookingAround = true;
    }
    public void GoToNextPoint()
    {
        index++;
        index = index % PatrolPoints.Count;
        owner.SetAITarget(PatrolPoints[index]);
    }
    public void TransToAttackState()
    {
        machine.TranslateToState(2);
    }
}
