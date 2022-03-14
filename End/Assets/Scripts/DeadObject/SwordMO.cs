using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMO : MoveObject
{
   
    public override void Attack(Vector2 target)
    {
        PlayAnim("attack");
        Invoke(nameof(CheckAttack), 0.4f);
    }
    void CheckAttack()
    {
        foreach (MoveObject mo in Game.instance.curEnemies)
        {
            if (mo != null)
            {
                if (Vector2.Distance(mo.transform.position, transform.position)<attackRadius)
                {
                    print(mo.gameObject.name);
                    mo.GetHurt(51, (mo.transform.position - transform.position).normalized);
                }
            }
        }
    }
}
