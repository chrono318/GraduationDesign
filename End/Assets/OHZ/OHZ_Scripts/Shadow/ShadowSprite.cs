using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    Transform player;
    SpriteRenderer thisSprite;
    SpriteRenderer playerSprite;

    SpriteRenderer[] renderers;

    Color color;

    [Header("时间控制参数")]
    public float activeTime = 1;//显示残影的时长
    public float startTime;//开始显示的时间

    [Header("不透明度控制")]
    private float alpha;
    public float alphaSet = 0.9f;//初始Alpha值
    public float alphaMul = 0.8f;

    private void OnEnable()
    {
        alpha = alphaSet;
        startTime = Time.time;
    }

    public void UpdateRendererSprites(SpriteRenderer[] spriteRenderers) 
    {
        print(spriteRenderers.Length);
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            transform.GetChild(i).position = spriteRenderers[i].transform.position;
            transform.GetChild(i).localScale = spriteRenderers[i].transform.localScale;
            transform.GetChild(i).rotation = spriteRenderers[i].transform.rotation;
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = spriteRenderers[i].sprite;
        }
    }
    void Update()
    {
        alpha *= alphaMul;
        color = new Color(0.5f, 0.5f, 1, alpha);
        //thisSprite.color = color;
        for(int i = 0; i < transform.GetChildCount(); i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = color;
        }
        if (Time.time >= activeTime + startTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
