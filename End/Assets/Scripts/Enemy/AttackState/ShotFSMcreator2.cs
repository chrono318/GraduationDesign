using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFSMcreator2 : FSMcreator
{
    [Tooltip("尿分叉程度（角度）")]
    public float ShotOffset = 30f;
    public GameObject bulletForEnemy;
    public GameObject bulletForPlayer;
    public Transform bulletOriPos;
    public override void AddOtherStates()
    {
        Default_Vigilance default_Vigilance = new Default_Vigilance(1, enemy);
        ShotAttackState2 shotAttackState2 = new ShotAttackState2(2, enemy);

        shotAttackState2.creator = this;
        attackState = shotAttackState2;

        machine.AddState(default_Vigilance);
        machine.AddState(shotAttackState2);
    }
    public override void SetPlayer(Player player)
    {
        base.SetPlayer(player);
        gameObject.tag = "Player";
        timing.SetAttackSpace(0f);
    }
    private void Update()
    {
        //Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //targetPos = new Vector3(targetPos.x, targetPos.y, bone.position.z);
        //bone.rotation = Quaternion.FromToRotation(Vector3.right, targetPos - bone.position);
    }
}

public class ShotAttackState2 : Default_AttackState
{
    public ShotFSMcreator2 creator;
    public ShotAttackState2(int id, Enemy o) : base(id, o)
    {

    }
    public override void Attack()
    {
        base.Attack();
        owner.animator.SetTrigger("attack");
        Quaternion quaternion;
        Vector3 tar;
        if (machine.isplayer)
        {
            tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tar = new Vector3(tar.x, tar.y, creator.bulletOriPos.position.z);
            float a = Vector2.SignedAngle(Vector2.right, tar - creator.bulletOriPos.position);

            //尿分叉
            float offset = creator.ShotOffset;
            float r = Random.Range(-offset, offset);
            //
            quaternion = Quaternion.Euler(0, 0, a + r);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

        }
        else
        {
            tar = owner.target.transform.position;
            tar = new Vector3(tar.x, tar.y, creator.bulletOriPos.position.z);
            quaternion = Quaternion.FromToRotation(Vector3.right, tar - creator.bulletOriPos.position);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);
        }
        
    }
}

