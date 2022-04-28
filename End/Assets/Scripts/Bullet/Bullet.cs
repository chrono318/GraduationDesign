using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 dir;
    public float speed = 5f;
    public bool isPlayer = false;
    public Pool pool;
    public void Init(Vector2 dir,float speed,bool isplayer,Pool pool)
    {
        gameObject.SetActive(true);
        this.dir = dir.normalized;
        this.speed = speed;
        this.isPlayer = isplayer;
        this.pool = pool;
        GetComponent<Animator>().SetTrigger("reset");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyBulletSelf()
    {
        pool.ReturnGoToPool(gameObject);
    }
}
