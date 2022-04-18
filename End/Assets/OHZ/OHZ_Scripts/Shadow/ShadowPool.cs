using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;

    public int shadowCount = 10;

    Queue<GameObject> availableObjects = new Queue<GameObject>();

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    /// <summary>
    /// 填充对象池
    /// </summary>
    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            ReturnPool(newShadow);
        }
    }

    /// <summary>
    /// 返回对象池
    /// </summary>
    /// <param name="gameObject"></param>
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        availableObjects.Enqueue(gameObject);
    }

    /// <summary>
    /// 取出对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetFormPool(SpriteRenderer[] spriteRenderers)//#对接#给谁用就调用这个函数就行#对接#
    {
        if (!canLeaveCanyin) return null;
        canLeaveCanyin = false;
        if (availableObjects.Count == 0)
        {
            FillPool();
        }
        var outShadow = availableObjects.Dequeue();
        outShadow.GetComponent<ShadowSprite>().UpdateRendererSprites(spriteRenderers);
        outShadow.SetActive(true);

        return outShadow;
    }

    public void SetSprites(SpriteRenderer[] spriteRenderers)
    {

    }

    float t = 0f;
    bool canLeaveCanyin = true;
    private void Update()
    {
        if (!canLeaveCanyin)
        {
            t += Time.deltaTime;
            if (t > 0.1f)
            {
                canLeaveCanyin = true;
                t = 0f;
            }
        }
    }
}
