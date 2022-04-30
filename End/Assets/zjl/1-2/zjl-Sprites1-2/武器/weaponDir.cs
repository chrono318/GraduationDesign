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
        enabled = false;
        controller.weaponDir = this;
    }
    private void LateUpdate()
    {
        if (!update) return;
        Vector2 dir = (target - (Vector2)center.position);
        float len = Mathf.Min(length, dir.magnitude);
        dir = dir.normalized;
        weapon.position = dir * len + (Vector2)center.position;
        weapon1.position = weapon.position;
        Vector2 oriDir = new Vector2(-1 * (controller.GetMoveObject().transform.GetChild(0).localScale.x > 0 ? 1 : -1), 1);

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
}
