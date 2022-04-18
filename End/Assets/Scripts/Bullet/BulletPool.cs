using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public int BulletNumPerInstantiate = 5;
    public GameObject BulletPrefab;
    [HideInInspector]
    public static BulletPool instance;
    Queue<Bullet> availableBullets; //目前可用的子弹

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        availableBullets = new Queue<Bullet>();
        FillBulletPool();
    }
    /// <summary>
    /// 对象池中可用子弹数不够时，此函数填充availableBullets，个数为BulletNumPerInstantiate
    /// </summary>
    private void FillBulletPool()
    {
        for(int i = 0; i < BulletNumPerInstantiate; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab,transform);
            availableBullets.Enqueue(bullet.GetComponent<Bullet>());
        }
    }
    /// <summary>
    /// 生成一颗子弹，自动进入子弹池接受管理
    /// </summary>
    /// <returns></returns>
    public Bullet CreateBullet(Bullet type, Vector2 oriPos, Vector2 oriDir, float oriSpeed)
    {
        Bullet bullet;
        if (availableBullets.Count <= 0)
        {
            FillBulletPool();
        }
        bullet = availableBullets.Dequeue();

        return bullet;
    }
    public void ReturnBulletPool(Bullet bullet)
    {

    }
}
