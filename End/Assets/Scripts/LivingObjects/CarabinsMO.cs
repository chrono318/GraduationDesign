using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarabinsMO : MoveObject
{
    public GameObject bulletForEnemy;
    public GameObject bulletForPlayer;
    public override void Attack(Vector2 target)
    {
        PlayAnim("attack");
        Quaternion quaternion;
        float gunAngle = Vector2.SignedAngle(Vector2.right, target - (Vector2)attackPoint.position);
        quaternion = Quaternion.Euler(0, 0, gunAngle);

        if (isPlayer)
        {
            GameObject.Instantiate(bulletForPlayer, attackPoint.position, quaternion);
        }
        else
        {
            GameObject.Instantiate(bulletForEnemy, attackPoint.position, quaternion);
        }
    }
}
