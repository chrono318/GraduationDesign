using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected MoveObject moveObject;
    protected MoveObjectType curType;
    protected Rigidbody2D rigidbody;
    protected void SetMoveObject(MoveObject moveObject)
    {
        this.moveObject = moveObject;
        this.curType = moveObject.type;
        moveObject.SetController(this);
        rigidbody = moveObject.GetRigidBody();
    }

    /// <summary>
    /// transform移动
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void MoveUpdate(Vector2 dir, float speedScale)
    {
        moveObject.MoveUpdate(dir, speedScale);
    }
    /// <summary>
    /// rigidbody移动
    /// </summary>
    /// <param name="speed"></param>
    public void MoveVelocity(Vector2 speed)
    {
        moveObject.MoveVelocity(speed);
    }
    /// <summary>
    /// 控制图片转向，虚函数，可override
    /// </summary>
    /// <param name="isleft"></param>
    public void TurnTowards(bool isleft)
    {
        moveObject.TurnTowards(isleft);
    }
}