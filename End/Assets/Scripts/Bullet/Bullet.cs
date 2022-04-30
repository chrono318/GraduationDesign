using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public bool isPlayer = false;
    public Pool pool;

    protected bool col = false;
    public void Init(Vector3 position,Quaternion rotation,float speed,bool isplayer,Pool pool)
    {
        transform.position = position;
        transform.rotation = rotation;
        this.speed = speed;
        this.isPlayer = isplayer;
        this.pool = pool;
        col = false;
        GetComponent<Animator>().SetTrigger("reset");
        gameObject.SetActive(true);
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
