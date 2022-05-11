using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class weaponDir : MonoBehaviour
{
    public Controller controller;
    [HideInInspector]
    public Vector2 target;
    public Transform weapon;
    public Transform weapon1;
    public Transform center;
    public int oriAngle = 0;
    public float length = 5f;
    public bool _update = true;

    private void Start()
    {
        controller.weaponDir = this;
        DisableUpdate();
    }
    private void Update()
    {
        if (!_update) return;
        Vector2 dir = (target - (Vector2)center.position);
        float len = Mathf.Min(length, dir.magnitude);
        dir = dir.normalized;
        weapon.position = dir * len + (Vector2)center.position;
        weapon1.position = weapon.position;
        //Vector2 oriDir = new Vector2(controller.GetMoveObject().transform.GetChild(0).localScale.x > 0 ? 1 : -1, 0);
        Vector2 oriDir = new Vector2(transform.localScale.x * -1, 0);
        oriDir = Quaternion.AngleAxis(oriAngle * oriDir.x, Vector3.forward) * (Vector3)oriDir;

        weapon.rotation = Quaternion.FromToRotation(oriDir, dir);
        weapon1.rotation = Quaternion.FromToRotation(oriDir, dir);
    }

    public void AttackShake(float time,float shakeDistance,Vector2 target)
    {
        //update = false;
        weapon.position -= (Vector3)(target - (Vector2)center.position).normalized * shakeDistance;
        weapon1.position = weapon.position;
        //Invoke(nameof(BackToUpdate), time);
    }
    
    public void BackToUpdate()
    {
        enabled = true;
        _update = true;
        GetComponent<IKManager2D>().enabled = true;
    }

    public void DisableUpdate()
    {
        enabled = false;
        _update = false;
        GetComponent<IKManager2D>().enabled = false;
    }
}
