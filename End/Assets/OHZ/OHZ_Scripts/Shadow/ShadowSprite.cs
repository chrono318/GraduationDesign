using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    Transform player;
    SpriteRenderer thisSprite;
    SpriteRenderer playerSprite;

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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.gameObject.GetComponent<SpriteRenderer>();
        alpha = alphaSet;

        //用在想要残影效果的对象上
        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        startTime = Time.time;
    }

    void Update()
    {
        alpha *= alphaMul;
        color = new Color(0.5f, 0.5f, 1, alpha);
        thisSprite.color = color;
        if (Time.time >= activeTime + startTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
