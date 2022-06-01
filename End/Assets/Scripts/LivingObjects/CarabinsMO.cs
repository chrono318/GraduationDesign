using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarabinsMO : MoveObject
{
    public bool isShiRen = false;
    public override void Attack(Vector2 target)
    {
        base.Attack(target);
        PlayAnim("attack");
        Quaternion rota = Quaternion.FromToRotation(Vector3.right, (Vector3)target - attackPoint.position);
        CreateBullet(attackPoint.position, rota);
        if (!isShiRen)
            //SoundManager.instance.PlaySoundClipRandom(10, 15, SoundManager.instance.combatSound);
            audioSource.PlayOneShot(SoundManager.instance.GetSoundClipRandom(10, 15, SoundManager.instance.combatSound));
        else
            //SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[16]);
            audioSource.PlayOneShot(SoundManager.instance.combatSound[16]);
    }

    protected override void ReloadSound()
    {
        if (isShiRen)
            //SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[20]);
            audioSource.PlayOneShot(SoundManager.instance.combatSound[20]);
        else
            //SoundManager.instance.PlaySoundClip(SoundManager.instance.combatSound[17]);
            audioSource.PlayOneShot(SoundManager.instance.combatSound[17]);
    }
}
