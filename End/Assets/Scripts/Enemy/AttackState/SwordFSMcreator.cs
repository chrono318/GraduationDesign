using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFSMcreator : FSMcreator
{
    public ParticleSystem Left;
    public ParticleSystem Right;
    public Vector2 Start_Duration;
    public override void AddOtherStates()
    {
        Default_Vigilance default_Vigilance = new Default_Vigilance(1, enemy);
        SwordAttackState swordAttackState = new SwordAttackState(2, enemy);

        swordAttackState.creator = this;
        attackState = swordAttackState;

        machine.AddState(default_Vigilance);
        machine.AddState(swordAttackState);
    }
    public void Attack(bool left)
    {
        if (left)
        {
            Left.Play();
            StartCoroutine(nameof(LeftColli));
        }
        else
        {
            Right.Play();
            StartCoroutine(nameof(RightColli));
        }
    }
    IEnumerator LeftColli()
    {
        yield return new WaitForSeconds(Start_Duration.x);
        //Left.GetComponent<Collider2D>().enabled = true;
        Left.GetComponent<Attack_Item>().CheckAttack();
        yield return new WaitForSeconds(Start_Duration.y);
        //Left.GetComponent<Collider2D>().enabled = false;
        OnFinishAttack();
    }
    IEnumerator RightColli()
    {
        yield return new WaitForSeconds(Start_Duration.x);
        //Right.GetComponent<Collider2D>().enabled = true;
        Right.GetComponent<Attack_Item>().CheckAttack();
        yield return new WaitForSeconds(Start_Duration.y);
        //Right.GetComponent<Collider2D>().enabled = false;
        OnFinishAttack();
    }
    
}

public class SwordAttackState : Default_AttackState
{
    public SwordFSMcreator creator;
    public SwordAttackState(int id, Enemy o) : base(id, o)
    {

    }
    public override void Attack()
    {
        if (machine.isplayer)
        {
            machine.player.SetStateAttack();
        }
        //owner.animator.SetTrigger("attack1");
        owner.PlayAnima("attack");

        machine.TranslateToState(1);
        bool isleft = owner.GFX.localScale.x == 1 ? false : true;
        creator.Attack(isleft);
    }
}
