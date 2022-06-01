using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrels : MonoBehaviour
{
    public AudioSource audioSource;
    public float radius = 5f;//爆炸范围
    public LayerMask targetLayer;
    public float boomForce = 10f;

    public ParticleSystem particleSystem;
    public float damage = 50f;
    public void CloseShadow()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        particleSystem.Play();
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    public void Boom()
    {
        //SoundManager.instance.PlaySoundClip(SoundManager.instance.effectSound[7]);
        audioSource.PlayOneShot(SoundManager.instance.effectSound[7]);
        Collider2D[] arround = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (var hit in arround)
        {
            //#对接#
            //看看击退效果合不合适，加入受伤判定
            //#对接#
            Vector3 pos = transform.position - hit.transform.position;
            //hit.GetComponent<Rigidbody2D>().AddForce(-pos.normalized * boomForce, ForceMode2D.Impulse);
            if (hit.gameObject.TryGetComponent<MoveObject>(out MoveObject moveObject))
            {
                if (moveObject.type == MoveObjectType.Dead) continue;
                moveObject.GetHurt(damage, -pos.normalized * boomForce,false);
            }
        }

    }

    public void DestoryBarren()
    {
        Destroy(transform.parent.gameObject);
    }
    
    public void CallBoomAnim()
    {
        GetComponent<Animator>().Play("bomb");
    }
}
