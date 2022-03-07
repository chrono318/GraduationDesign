using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [Tooltip("跑动时玩家速度/原物体速度")]
    public float runSpeedScale = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //移动
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dirSpeed = new Vector2(h, v);
        MoveUpdate(dirSpeed, runSpeedScale);
        //朝向
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dir = mousePos.x - transform.position.x;
        TurnTowards(dir < 0);
        //攻击（左键）/技能（右键）
        if (Input.GetMouseButton(0))
        {
            moveObject.MouseBtnLeft();
        }
        else if (Input.GetMouseButton(1))
        {
            moveObject.MouseBtnRight();
        }
    }

}
