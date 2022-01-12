using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    //整合 —— 如果场景中没有敌人则显示小地图，有敌人则不显示（玩家附身对象不计入其中）
    public GameObject miniMap;
    bool miniMapSwitch = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMapSwitch = !miniMapSwitch;
            miniMap.SetActive(miniMapSwitch);
        }
    }
}
