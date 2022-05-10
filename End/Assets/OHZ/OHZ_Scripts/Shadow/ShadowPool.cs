using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;

    public int shadowCount = 10;

    Queue<Transform> availableShadows = new Queue<Transform>();
    List<Transform> managedShadows = new List<Transform>();
    int count = 0;
    Transform[] shadows;
    int curIndex = 0;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    /// <summary>
    /// 填充对象池
    /// </summary>
    //public void FillPool()
    //{
    //    for (int i = 0; i < shadowCount; i++)
    //    {
    //        var newShadow = Instantiate(shadowPrefab);
    //        newShadow.transform.SetParent(transform);

    //        ReturnPool(newShadow);
    //    }
    //}

    /// <summary>
    /// 返回对象池
    /// </summary>
    /// <param name="gameObject"></param>
    //public void ReturnPool(GameObject gameObject)
    //{
    //    gameObject.SetActive(false);
    //    availableObjects.Enqueue(gameObject);
    //}

    public void ReturnPool(Transform whiteRoot)
    {
        whiteRoot.gameObject.SetActive(false);
        //availableShadows.Enqueue(whiteRoot);
        //managedShadows.Remove(whiteRoot);
    }

    /// <summary>
    /// 取出对象
    /// </summary>
    /// <returns></returns>
    //public GameObject GetFormPool(SpriteRenderer[] spriteRenderers)//#对接#给谁用就调用这个函数就行#对接#
    //{
    //    if (!canLeaveCanyin) return null;
    //    canLeaveCanyin = false;
    //    if (availableObjects.Count == 0)
    //    {
    //        FillPool();
    //    }
    //    var outShadow = availableObjects.Dequeue();
    //    outShadow.GetComponent<ShadowSprite>().UpdateRendererSprites(spriteRenderers);
    //    outShadow.SetActive(true);

    //    return outShadow;
    //}
    public void RegisterCanying(GameObject whiteRoot,int count)//,Vector3 position,Quaternion rotation,Vector3 scale)
    {
        //clear the old
        for(int n = 0; n < transform.childCount; n++)
        {
            Destroy(transform.GetChild(n).gameObject);
        }

        this.count = count;
        shadows = new Transform[count];
        for(int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(whiteRoot, transform, true);
            go.AddComponent<ShadowSprite>();
            shadows[i] = go.transform;
            curIndex = 0;
            go.SetActive(false);
            //ReturnPool(go.transform);
        }
    }
    public void SetCanying(Transform whiteRoot)
    {
        if (!canLeaveCanyin) return;
        canLeaveCanyin = false;
        Transform shadow = shadows[curIndex];
        curIndex++;
        curIndex %= count;
        managedShadows.Add(shadow);
        shadow.gameObject.SetActive(true);
        shadow.position = whiteRoot.position;
        //从根骨骼开始copy
        CopyTransform(whiteRoot.GetChild(0), shadow.GetChild(0));
    }

    private void CopyTransform(Transform whiteRoot,Transform shadow)
    {
        shadow.position = whiteRoot.position;
        shadow.rotation = whiteRoot.rotation;
        shadow.localScale = whiteRoot.localScale;
        for(int i = 0; i < whiteRoot.childCount; i++)
        {
            CopyTransform(whiteRoot.GetChild(i), shadow.GetChild(i));
        }
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
