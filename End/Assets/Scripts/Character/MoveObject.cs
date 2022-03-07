using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public MoveObjectType type;
    //控制图片整体，控制朝向
    [Header("图片")]
    public Transform GFX;
    public Animator[] animators;

    public float speed = 10f;

    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    protected Controller controller;
    private void Reset()
    {
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        collider = gameObject.AddComponent<Collider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void MouseBtnLeft()
    {

    }
    public virtual void MouseBtnRight()
    {

    }
    public void MoveUpdate(Vector2 dir , float speedScale)
    {
        Vector3 detal = (Vector3)dir * speed * speedScale * Time.deltaTime;
        transform.position += detal;
        //TurnTowards(dir.x < 0);
    }
    public void MoveVelocity(Vector2 speed)
    {
        rigidbody.velocity = speed;
        //TurnTowards(speed.x < 0);
    }
    public virtual void TurnTowards(bool isleft)
    {
        GFX.localScale = new Vector3(isleft ? 1 : -1, 1, 1);
    }
    public void SetController(Controller controller)
    {
        this.controller = controller;
    }
    public Rigidbody2D GetRigidBody()
    {
        return rigidbody;
    }
}

/// <summary>
/// 死物还是活物
/// </summary>
public enum MoveObjectType
{
    Living,
    Dead
}

