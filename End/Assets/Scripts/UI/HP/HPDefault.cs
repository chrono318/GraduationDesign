using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPDefault : MonoBehaviour
{
    public Slider slider_main;
    public Slider slider_bg;
    public float duration = 0.7f;
    
    //变化调用函数
    public void SetValue(float value)
    {
#if UNITY_EDITOR
        if(value<0 || value > 1)
        {
            Debug.Log("value is illegal");
        }
#endif
        slider_main.value = value;
        slider_bg.DOValue(value, duration);
    }
}
