using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarabinsMO : MoveObject
{
    public override void Attack(Vector2 target)
    {
        PlayAnim("attack");
  
        CreateBullet(attackPoint.position, target - (Vector2)attackPoint.position);
    }
}
