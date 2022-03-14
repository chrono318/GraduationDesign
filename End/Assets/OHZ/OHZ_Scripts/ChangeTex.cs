using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTex : MonoBehaviour
{
    public Sprite[] picture;
    int id = 0;
    public Image mySelf;
    public GameObject gongFengUI;


    private void Update()
    {
        if (id < picture.Length)
        {
            mySelf.sprite = picture[id];
        }
        //mySelf.sprite = picture[id];
    }

    public void ChangeTexture()
    {
        id += 1;
    }

    public void CloseUI()
    {
        gongFengUI.SetActive(false);
    }
}
