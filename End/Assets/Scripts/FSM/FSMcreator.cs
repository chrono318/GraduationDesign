using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMcreator : MonoBehaviour
{
    [Header("巡逻点")]
    public List<Transform> PatrolPoints;
    protected StateMachine machine;
    protected Enemy enemy;
    public Default_AttackState attackState;

    [Header("攻击")]
    [Tooltip("攻击范围")]
    public float attack_radius;
    [Tooltip("弹匣容量，默认为0（近战）")]
    public int bulletNum = 0;
    [Tooltip("换弹匣时间（近战为0，默认为0）")]
    public float shotReload = 0f;
    public float attackSpace = 2f;

    //
    public AttackTiming timing;
    public virtual void SetPlayer(Player player)
    {
        machine.isplayer = true;
        machine.player = player;
    }
    public StateMachine CreateSFM()
    {
        enemy = GetComponent<Enemy>();

        Default_PatrolState default_PatrolState = new Default_PatrolState(0, enemy);
        default_PatrolState.PatrolPoints = PatrolPoints;
        enemy.enemyAI.OnReach.AddListener(default_PatrolState.OnReach);

        machine = new StateMachine(default_PatrolState);

        //Add Other States
        AddOtherStates();

        timing = new AttackTiming(bulletNum, shotReload, attackSpace);

        return machine;
    }
    public virtual void AddOtherStates()
    {

    }

    public void OnFinishAttack()
    {
        if (machine.isplayer)
        {
            machine.player.OnAttackFinish();
        }
    }
    
}
