using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarabinsMO : MoveObject
{
    public override void Attack(Vector2 target)
    {
        PlayAnim("attack");
        Quaternion rota = Quaternion.FromToRotation(Vector3.right, (Vector3)target - attackPoint.position);
        CreateBullet(attackPoint.position, rota);
    }
}
