using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [HideInInspector]
    public static PoolManager instance;

    public List<Pool> pools;

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
        pools = new List<Pool>();
    }

    public void RemovePool(Pool pool)
    {
        pools.Remove(pool);
    }
    /// <summary>
    /// 注册一个对象池
    /// </summary>
    /// <param name="prefab">对象池里的预制体</param>
    public Pool RegisterPool(GameObject prefab)
    {
        foreach(Pool pool in pools)
        {
            if(pool.prefab == prefab)
            {
                pool.Reference++;
                return pool;
            }
        }
        GameObject newPoolGO = new GameObject(prefab.name+"Pool");
        newPoolGO.transform.parent = transform;
        Pool pool1 = newPoolGO.AddComponent<Pool>();
        pool1.prefab = prefab;
        pools.Add(pool1);
        pool1.Reference++;
        return pool1;
    }
    /// <summary>
    /// 卸载一个对象池
    /// </summary>
    /// <param name="pool"></param>
    public void LogOutPool(Pool pool)
    {
        pool.Reference--;
    }
}
