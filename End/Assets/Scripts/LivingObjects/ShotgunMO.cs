using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunMO : MoveObject
{
    public GameObject PlayerBullet;
    public GameObject EnemyBullet;
    public override void Attack(Vector2 target)
    {
        PlayAnim("attack");
        float gunAngle = Vector2.SignedAngle(Vector2.right, target - (Vector2)attackPoint.position);
        if (gunAngle > 30)
        {
            PlayAnim("upAttack");
        }
        else if (gunAngle < -30)
        {
            PlayAnim("downAttack");
        }
        else
        {
            PlayAnim("attack");
        }

        Quaternion quaternion;
        if (isPlayer)
        {
            quaternion = Quaternion.Euler(0, 0, 30 + gunAngle);
            GameObject.Instantiate(PlayerBullet, attackPoint.position, quaternion);

            //r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, -30 + gunAngle );
            GameObject.Instantiate(PlayerBullet, attackPoint.position, quaternion);

            //r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, 10 + gunAngle );
            GameObject.Instantiate(PlayerBullet, attackPoint.position, quaternion);

            //r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, -10 + gunAngle );
            GameObject.Instantiate(PlayerBullet, attackPoint.position, quaternion);
        }
        else
        {
            quaternion = Quaternion.Euler(0, 0, 30 + gunAngle);
            GameObject.Instantiate(EnemyBullet, attackPoint.position, quaternion);

            //r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, -30 + gunAngle);
            GameObject.Instantiate(EnemyBullet, attackPoint.position, quaternion);

            //r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, 10 + gunAngle);
            GameObject.Instantiate(EnemyBullet, attackPoint.position, quaternion);

            //r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, -10 + gunAngle);
            GameObject.Instantiate(EnemyBullet, attackPoint.position, quaternion);
        }
    }
}
