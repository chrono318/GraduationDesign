using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_FSMcreator : FSMcreator
{
    public override void AddOtherStates()
    {
        Default_Vigilance default_Vigilance = new Default_Vigilance(1, enemy);
        Default_AttackState default_AttackState = new Default_AttackState(2, enemy);

        //
        attackState = default_AttackState;

        machine.AddState(default_Vigilance);
        machine.AddState(default_AttackState);
    }
}
