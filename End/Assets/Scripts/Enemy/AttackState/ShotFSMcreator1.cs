using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFSMcreator1 : FSMcreator
{
    [Tooltip("尿分叉程度（角度）")]
    public float ShotOffset = 30f;
    public GameObject bulletForEnemy;
    public GameObject bulletForPlayer;
    public Transform bulletOriPos;

    public float playerAttackSpace = 1f;
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
        timing.SetReloadTime(playerAttackSpace);
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
        //owner.animator.SetTrigger("attack");
        Quaternion quaternion;
        Vector3 tar;
        if (machine.isplayer)
        {
            tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tar = new Vector3(tar.x, tar.y, creator.bulletOriPos.position.z);
            float a = Vector2.SignedAngle(Vector2.right, tar - creator.bulletOriPos.position);
            float b = Vector2.Angle(Vector2.right, tar - creator.bulletOriPos.position);

            if (a > 30)
            {
                owner.PlayAnima("upAttack");
            }
            else if (a < -30)
            {
                owner.PlayAnima("downAttack");
            }
            else
            {
                owner.PlayAnima("attack");
            }
            //尿分叉
            float offset = creator.ShotOffset;
            float r = Random.Range(-offset, offset);
            //

            quaternion = Quaternion.Euler(0, 0, 30 + a + r);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

            r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, -30 + a + r);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

            r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, 10 + a + r);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

            r = Random.Range(-offset, offset);
            quaternion = Quaternion.Euler(0, 0, -10 + a + r);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);

        }
        else
        {
            owner.PlayAnima("attack");
            tar = owner.target.transform.position;
            tar = new Vector3(tar.x, tar.y, creator.bulletOriPos.position.z);
            float a = Vector2.SignedAngle(Vector3.right, tar - creator.bulletOriPos.position);
            quaternion = Quaternion.Euler(0,0,45+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.Euler(0, 0, -45+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.Euler(0, 0, 15+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);

            quaternion = Quaternion.Euler(0, 0, -15+a);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);
        }
        
    }
}

