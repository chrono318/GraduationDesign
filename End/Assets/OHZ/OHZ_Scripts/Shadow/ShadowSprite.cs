using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ShadowSprite : MonoBehaviour
{
    Transform player;
    SpriteRenderer thisSprite;
    SpriteRenderer playerSprite;

    SpriteRenderer[] renderers;
    public Color oriColor = new Color(0, 0.9803922f, 1, 1);

    Color color;

    [Header("时间控制参数")]
    public float activeTime = 1;//显示残影的时长
    public float startTime;//开始显示的时间

    [Header("不透明度控制")]
    private float alpha;
    public float alphaSet = 1f;//初始Alpha值
    public float alphaMul = 0.95f;

    public bool update = true;
    private void Awake()
    {
        color = oriColor;
        renderers = transform.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = color;
        }
    }
    private void OnEnable()
    {
        alpha = alphaSet;
        startTime = Time.time;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = color;
        }
    }
    void Update()
    {
        alpha *= alphaMul;
        color = oriColor * alpha;
        //thisSprite.color = color;
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = color;
        }
        if (Time.time >= activeTime + startTime)
        {
            //返回对象池
            //ShadowPool.instance.ReturnPool(this.gameObject);
            gameObject.SetActive(false);
        }
    }
}
