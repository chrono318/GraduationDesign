using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool update = true;

    private void Start()
    {
        this.enabled = false;
        controller.weaponDir = this;
        update = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            print("press");
            this.enabled = false;
        }
        if (!update) return;
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
        update = false;
        weapon.position -= (Vector3)(target - (Vector2)center.position).normalized * shakeDistance;
        weapon1.position = weapon.position;
        Invoke(nameof(BackToUpdate), time);
    }
    
    void BackToUpdate()
    {
        update = true;
    }

    //private void OnEnable()
    //{
    //    print("on enable");
    //}
    //private void OnDisable()
    //{
    //    print("on disable");
    //}
}
