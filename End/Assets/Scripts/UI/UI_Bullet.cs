using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bullet : MonoBehaviour
{
    public Text surplus;
    public Text sum;
    
    public void UpdateNum(int surplusNum,int sumNum)
    {
        surplus.text = surplusNum.ToString();
        sum.text = sumNum.ToString();
    }
}
