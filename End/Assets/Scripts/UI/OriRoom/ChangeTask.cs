using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTask : MonoBehaviour
{
    public Text text;
    private bool ori = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        if (ori)
        {
            text.text = "造成<color=#00FF01FF>100</color>伤害";
            ori = false;
        }
        else
        {
            text.text = "击杀<color=#00FF01FF>3</color>名敌人";
            ori = true;
        }
    }
}
