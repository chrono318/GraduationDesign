using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFSMcreator1 : FSMcreator
{
    public GameObject bulletForEnemy;
    public GameObject bulletForPlayer;
    public Transform bulletOriPos;
    public override void AddOtherStates()
    {
        Default_Vigilance default_Vigilance = new Default_Vigilance(1, enemy);
        ShotAttackState1 shotAttackState1 = new ShotAttackState1(2, enemy);

        shotAttackState1.creator = this;
        attackState = shotAttackState1;

        machine.AddState(default_Vigilance);
        machine.AddState(shotAttackState1);
    }
    public override void SetPlayer(Player player)
    {
        base.SetPlayer(player);
        gameObject.tag = "Player";
    }
    private void Update()
    {
        //Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //targetPos = new Vector3(targetPos.x, targetPos.y, bone.position.z);
        //bone.rotation = Quaternion.FromToRotation(Vector3.right, targetPos - bone.position);
    }
}

public class ShotAttackState1 : Default_AttackState
{
    public ShotFSMcreator1 creator;
    public ShotAttackState1(int id, Enemy o) : base(id, o)
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
            quaternion = Quaternion.FromToRotation(Vector3.right + Vector3.up * 1.1f, tar - creator.bulletOriPos.position);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.FromToRotation(Vector3.right + Vector3.up *0.9f, tar - creator.bulletOriPos.position);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.FromToRotation(Vector3.right + Vector3.up *1.3f, tar - creator.bulletOriPos.position);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.FromToRotation(Vector3.right + Vector3.up * 0.7f, tar - creator.bulletOriPos.position);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);
        }
        else
        {
            tar = owner.target.transform.position;
            tar = new Vector3(tar.x, tar.y, creator.bulletOriPos.position.z);
            float a = Vector2.SignedAngle(Vector3.right, tar - owner.transform.position);
            quaternion = Quaternion.Euler(0,0,30+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.Euler(0, 0, -30+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.Euler(0, 0, 10+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.Euler(0, 0, -10+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);
        }
        
    }
}

