using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    private bool isPossess = false;
    public float ghostSpeed = 10f;
    [Tooltip("跑动时玩家速度/原物体速度")]
    public float runSpeedScale = 1.5f;
    public GameObject GFX;

    private CameraControl cameraControl;
    private List<MoveObject> DeadObjects;
    [HideInInspector]
    public float San = 100f;
    // Start is called before the first frame update
    void Start()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        DeadObjects = new List<MoveObject>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //附身逻辑 
        foreach (MoveObject ob in DeadObjects)
        {
            // draw line
            if((ob.transform.position - mousePos).magnitude < 2f)
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    StartCoroutine(Possess(ob));
                    break;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        //扣血
        if (moveObject.type == MoveObjectType.Dead || !isPossess)
        {
            San -= 0.1f;
            if (San <= 0)
            {
                // lose
            }
        }
        else
        {
            San += 0.05f;
            San = Mathf.Min(San, 100f);
        }

        //移动
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dirSpeed = new Vector2(h, v);

        //朝向
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - transform.position;

        if (isPossess)
        {
            //MoveUpdate(dirSpeed, runSpeedScale);
            MoveVelocity(dirSpeed * runSpeedScale);
            TurnTowards(dir.x < 0);
        }
        else
        {
            Vector3 detal = (Vector3)dir * dirSpeed * Time.deltaTime;
            transform.position += detal;
            transform.localScale = new Vector3(dir.x < 0 ? 1 : -1, 1, 1);
        }

        if (!isPossess) return;
        //攻击（左键）/技能（右键）
        if (Input.GetMouseButton(0))
        {
            if (moveObject.MouseBtnLeft())
            {
                Camera.main.GetComponent<CameraControl>().CameraShake(dir.normalized);//射击震动
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (moveObject.MouseBtnRight())
            {
                //翻滚后效？
            }
        }
    }
    public void GetHurtEffect(Vector2 force)
    {
        cameraControl.CameraShake(force.normalized);
    }
    public void AddDeadObject(MoveObject moveObject)
    {
        DeadObjects.Add(moveObject);
    }
    public void RemoveDeadObject(MoveObject moveObject)
    {
        DeadObjects.Remove(moveObject);
    }
    public void MoveObjectDead()
    {
        isPossess = false;
        GFX.SetActive(true);
    }
    private IEnumerator Possess(MoveObject moveObject)
    {
        RemoveDeadObject(moveObject);
        yield return new WaitForSeconds(2f);
        isPossess = true;
        GFX.SetActive(false);
        this.moveObject = moveObject;
        moveObject.SetController(this);
        moveObject.isPlayer = true;
    }
}
