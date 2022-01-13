using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Role
{
    public static Ghost instance;

    public float max_spiritPower = 100;
    public float spiritPower = 0;
    public float rush_distance = 3f;
    public float rush_duration = 0.5f;
    public Material m_material;

    public Enemy enemy;
    private bool canMove = true;
    public bool possessed = false;
    public bool hiding = true;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        spiritPower = max_spiritPower;

        offsets.Add(transform.position);
        offsets.Add(transform.position);
        offsets.Add(transform.position);
        offsets.Add(transform.position);

        m_material.SetFloat("_Hiding",0.3f);
    }

    bool rush = false;
    List<Vector3> offsets = new List<Vector3>();
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (possessed)
            {
                LeaveBody();
            }
            else
            {
                if (enemy)
                {
                    Possess();
                    enemy.CloseAI();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            hiding = !hiding;
            m_material.SetFloat("_Hiding", hiding ? 0.3f : 1.0f);
        }

        offsets.Add(transform.position);
        offsets.RemoveAt(0);
        
        if (rush)
        {
            m_material.SetVector("_Offset0", offsets[0] - transform.position);
            m_material.SetVector("_Offset1", offsets[1] - transform.position);
            m_material.SetVector("_Offset2", offsets[2] - transform.position);
            m_material.SetVector("_Offset3", offsets[3] - transform.position);
        }
        else
        {
            m_material.SetVector("_Offset0", transform.position);
            m_material.SetVector("_Offset1", transform.position);
            m_material.SetVector("_Offset2", transform.position);
            m_material.SetVector("_Offset3", transform.position);
        }

        if (!canMove)
            return;
        Move();

        
        //if (Input.GetMouseButton(0))
        //{
        //    Attack_chuizi();
        //    animator.SetTrigger("attack_chuizi");
        //}
        if (Input.GetMouseButton(1))
        {
            Attack_rush();
        }
    }

    [HideInInspector]
    public void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float t = Time.deltaTime;
        Vector3 move = new Vector3(h * speed * t, v * speed * t, 0);
        transform.position += move;

        float dir = h == 0 ? (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).x : h;
        transform.localScale = new Vector3((dir >= 0 ? -1 : 1), 1, 1);
        animator.SetBool("move", Vector3.Magnitude(move) > 0.01 ? true:false);
    }

    private void Possess()
    {
        canMove = false;

        GetComponent<Collider2D>().enabled = false;
        //GetComponent<Animator>().Play("Possess");

        OnFinishPossess();
    }
    public void OnFinishPossess()
    {
        transform.SetParent(enemy.transform);
        transform.localPosition = Vector3.zero;
        //enemy.canMove = true;

        animator.gameObject.SetActive(false);
        possessed = true;

        enemy.GetComponent<Enemy>().enabled = false;
        EnemyRevolt enemyRevolt =  enemy.gameObject.AddComponent<EnemyRevolt>();
        enemyRevolt.enemy = enemy;
        enemyRevolt.ghost = this;
        //enemy.GetComponent<Player>().animator = enemy.animator;

        Camera.main.GetComponent<CameraControl>().OpenProcessVolume();
    }
    public void LeaveBody()
    {
        animator.gameObject.SetActive(true);
        canMove = true;
        possessed = false;
        enemy.GetComponent<Enemy>().enabled = true;
        enemy.OpenAI();

        enemy.Dead();
        Destroy(enemy.GetComponent<EnemyRevolt>());
        transform.SetParent(enemy.transform.parent);
        GetComponent<Collider2D>().enabled = true;
        rush = false;

        Camera.main.GetComponent<CameraControl>().CloseProcessVolume();

    }
    public void LeaveBodyForced()
    {
        animator.gameObject.SetActive(true);
        canMove = true;
        possessed = false;
        GetComponent<Collider2D>().enabled = true;
        rush = false;
    }

    public chuizi chuizi;
    
    private void Attack_chuizi()
    {
        chuizi.gameObject.SetActive(true);
        canMove = false;
        chuizi.Finish += () => { this.canMove = true; };
    }

    private void Attack_rush()
    {
        canMove = false;
        animator.SetTrigger("roll");
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        StartCoroutine(StartRush(dir));

        rush = true;
        //material.SetVector("_Offset0", -dir);
    }

    private IEnumerator StartRush(Vector2 dir)
    {
        float len = 0f;
        while (len <= rush_distance)
        {
            float detal = rush_distance * Time.deltaTime / rush_duration;
            len += detal;
            transform.Translate(dir * detal, Space.World);
            yield return null;
        }

        animator.SetFloat("rush_num", -1);
        animator.SetTrigger("attack_rush");
        canMove = true;

        rush = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //传送
        print(collision.name);
        if(collision.TryGetComponent<DeliveryDoor>(out DeliveryDoor door))
        {
            door.GhostDelivery(this);
        }
        //
        if (rush)
        {
            collision.GetComponent<Role>().GetHurt(10, RoleType.San, Vector2.zero);
        }
        if (!canMove) return;
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponent<Enemy>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!canMove) return;
        if (collision.CompareTag("Enemy"))
        {
            enemy = null;
        }
    }
}
