using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XinWuTips : MonoBehaviour
{
    public GameObject panel;
    public Text text;
    public int level = 0;
    string[] strs = new string[5] { "攻击强化提升3%（3）/n", 
        "攻击强化提升百分之7%（6）/n", 
        "12%攻击强化（9）/n", 
        "15%攻击强化及精准（12）/n/n", 
        "攻击强化：附身状态下使用武器时的物理攻击力/n精准：子弹不再产生偏移" };
    // Start is called before the first frame update
    void UpdateText()
    {
        string sum = "<color=#00FF00>";
        for(int i = 0; i < 5; i++)
        {
            if (i == level)
            {
                sum += "</color>";
            }
            sum += strs[i];
        }
        text.text = sum;
    }

    // Update is called once per frame
    private void OnMouseEnter()
    {
        UpdateText();
        panel.SetActive(true);
    }
    private void OnMouseExit()
    {
        panel.SetActive(false);
    }
}
