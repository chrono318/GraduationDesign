using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFSMcreator : FSMcreator
{
    [Tooltip("尿分叉程度（角度）")]
    public float ShotOffset = 30f;
    public GameObject bulletForEnemy;
    public GameObject bulletForPlayer;
    public Transform bulletOriPos;
    public override void AddOtherStates()
    {
        Default_Vigilance default_Vigilance = new Default_Vigilance(1, enemy);
        ShotAttackState shotAttackState = new ShotAttackState(2, enemy);

        shotAttackState.creator = this;
        attackState = shotAttackState;

        machine.AddState(default_Vigilance);
        machine.AddState(shotAttackState);
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

public class ShotAttackState : Default_AttackState
{
    public ShotFSMcreator creator;
    public ShotAttackState(int id, Enemy o) : base(id, o)
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

            //尿分叉
            float a = Vector2.SignedAngle(Vector2.right, tar - creator.bulletOriPos.position);
            float offset = creator.ShotOffset;
            float r = Random.Range(-offset, offset);
            //
            quaternion = Quaternion.Euler(0, 0, a+r);
            GameObject.Instantiate(creator.bulletForPlayer, creator.bulletOriPos.position, quaternion);
        }
        else
        {
            tar = owner.target.transform.position;
            //修改面朝方向
            float m_dir = tar.x - owner.transform.position.x > 0 ? -1 : 1;
            owner.animator.transform.localScale = new Vector3(m_dir, 1, 1);

            tar = new Vector3(tar.x, tar.y, creator.bulletOriPos.position.z);
            quaternion = Quaternion.FromToRotation(Vector3.right, tar - creator.bulletOriPos.position);
            GameObject.Instantiate(creator.bulletForEnemy, creator.bulletOriPos.position, quaternion);
        }
        
    }
}

