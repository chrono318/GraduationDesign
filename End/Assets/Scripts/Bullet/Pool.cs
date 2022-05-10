using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private int reference = 0;
    public GameObject prefab;

    public int numPerInstantiate = 3;
    public List<GameObject> registor = new List<GameObject>();
    Queue<GameObject> availableGOs; //目前可用的子弹

    public int Reference
    {
        get => reference; set
        {
            reference = value;
            if (reference == 0)
            {
                Destroy(gameObject);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        availableGOs = new Queue<GameObject>();
        //registor = new List<GameObject>();
        FillBulletPool();
    }
    /// <summary>
    /// 对象池中可用对象不够时，此函数填充availableBullets，个数为BulletNumPerInstantiate
    /// </summary>
    private void FillBulletPool()
    {
        for(int i = 0; i < numPerInstantiate; i++)
        {
            GameObject go = Instantiate(prefab,transform);
            go.SetActive(false);
            availableGOs.Enqueue(go);
        }
    }
    /// <summary>
    /// 获取一个受管理的gameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        if (availableGOs.Count <= 0)
        {
            FillBulletPool();
        }

        return availableGOs.Dequeue();
    }
    public void ReturnGoToPool(GameObject go)
    {
        //没有判断是不是相同类型的gameObject
        go.SetActive(false);
        availableGOs.Enqueue(go);
    }

    private void OnDestroy()
    {
        PoolManager.instance.RemovePool(this);
    }
}
