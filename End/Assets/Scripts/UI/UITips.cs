using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITips : MonoBehaviour
{
    public Text text;
    public void ShowTips(string str)
    {
        text.text = str;
    }
    public void AAADestroy()
    {
        print("?????");
        Destroy(gameObject);
    }
}
