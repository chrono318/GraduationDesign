using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevolt : MonoBehaviour
{
    public float speed = 40f;
    public float control_force = 20f;
    public float ray_len = 20f;
    private float San_Max = 5f;
    private float San;
    private Rigidbody2D rigidbody;
    private Vector2 dir = Vector2.zero;
    [HideInInspector]
    public Enemy enemy;
    public Ghost ghost;
    private int touch_enemy_time = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<CameraControl>().OpenProcessVolume();
        dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        dir = dir.normalized;
        GetComponent<Enemy>().ghost.gameObject.SetActive(true);
        UIManager.instance.San.gameObject.SetActive(true);
        San = San_Max;
    }

    // Update is called once per frame
    void Update()
    {
        San -= Time.deltaTime * 5;
        if (San >= 0)
        {
            UIManager.instance.SetSanUI(San/San_Max);
        }
        else
        {
            gameObject.tag = "Player";
            Player p = gameObject.AddComponent<Player>();
            p.animator = enemy.animator;
            p.animators = enemy.animators;
            p.ghost = ghost;
            Game.instance.player = p;
            Destroy(this);
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 control = new Vector2(h , v);
        control -= Vector2.Dot(control, dir) * dir;
        Vector2 v2 = control * control_force + dir * speed;
        rigidbody.velocity = v2;

        enemy.GFX.localScale = new Vector3(v2.x > 0 ? -1 : 1, 1, 1);

        Debug.DrawLine(transform.position, transform.position + new Vector3(v2.x, v2.y, 0).normalized * ray_len, Color.red);
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position, v2,ray_len,LayerMask.GetMask("Default")))
        {
            UIManager.instance.ShowE(true, transform);

            if (Input.GetKey(KeyCode.E))
            {
                if (hit.normal.x == 0)
                {
                    dir = new Vector2(v2.x, -v2.y).normalized;
                }
                else
                {
                    dir = new Vector2(-v2.x, v2.y).normalized;
                }
                rigidbody.velocity = dir * speed;
            }
        }
        else
        {
            UIManager.instance.ShowE(false, transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().ShowHurt();
            touch_enemy_time++;
            if (touch_enemy_time >= 2)
            {
                Ghost.instance.LeaveBody();
                //UI_DamageManager.instance.DamageNum(20, DamageType.Magic, transform);
            }
        }
        else if (collision.gameObject.CompareTag("Soul Frag"))
        {
            GetSoulFrag(collision.gameObject);
        }
        else if (collision.gameObject.layer == 0)
        {
            Ghost.instance.LeaveBody();
            //UI_DamageManager.instance.DamageNum(20, DamageType.Magic, transform);
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        //enemy.animator.speed = Mathf.Clamp01(rigidbody.velocity.magnitude / 10);
        
    }
    private void GetSoulFrag(GameObject frag)
    {
        Destroy(frag);
    }

    private void OnDestroy()
    {
        GetComponent<Enemy>().ghost.gameObject.SetActive(false);
        UIManager.instance.ShowE(false, transform);
        UIManager.instance.San.gameObject.SetActive(false);
        Camera.main.GetComponent<CameraControl>().CloseProcessVolume();
    }
}
