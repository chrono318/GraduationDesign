using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testXInwu : MonoBehaviour
{
    public bool open = false;
    public Image image;
    private Color oriColor;
    // Start is called before the first frame update
    void Start()
    {
        oriColor = image.color;
    }


    public void Xinwu()
    {
        if (open)//关闭
        {
            open = false;
            image.color = oriColor;
            Game.instance.player.SetXinwu(false);
        }
        else
        {
            open = true;
            image.color = Color.clear;
            Game.instance.player.SetXinwu(true);
        }
    }
}
