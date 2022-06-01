using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunMO : MoveObject
{
    public bool lianfa = false;
    public override void Attack(Vector2 target)
    {
        base.Attack(target);
        float gunAngle = Vector2.SignedAngle(Vector2.right, target - (Vector2)attackPoint.position);
        if (lianfa)
        {
            PlayAnim("attack");
        }
        else if (gunAngle > 30)
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

        //SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[15]);
        audioSource.PlayOneShot(SoundManager.instance.combatSound[15]);

        Quaternion quaternion;
        quaternion = Quaternion.Euler(0, 0, 30 + gunAngle);
        CreateBullet(attackPoint.position, quaternion);

        //r = Random.Range(-offset, offset);
        quaternion = Quaternion.Euler(0, 0, -30 + gunAngle);
        CreateBullet(attackPoint.position, quaternion);

        //r = Random.Range(-offset, offset);
        quaternion = Quaternion.Euler(0, 0, 10 + gunAngle);
        CreateBullet(attackPoint.position, quaternion);

        //r = Random.Range(-offset, offset);
        quaternion = Quaternion.Euler(0, 0, -10 + gunAngle);
        CreateBullet(attackPoint.position, quaternion);
    }

    protected override void ReloadSound()
    {
        //SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[18]);
        audioSource.PlayOneShot(SoundManager.instance.combatSound[18]);
    }
}
