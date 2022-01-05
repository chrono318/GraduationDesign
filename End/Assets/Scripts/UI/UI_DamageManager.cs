using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DamageManager : MonoBehaviour
{
    public static UI_DamageManager instance;
    [HideInInspector]
    public UI_DamageNum[] Texts;
    private int index = 0;

    private void Reset()
    {
        Texts = transform.GetComponentsInChildren<UI_DamageNum>();
        foreach (UI_DamageNum num in Texts)
        {
            num.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        index = 0;
    }

    public void DamageNum(float damage, DamageType damageType,Transform position)
    {
        UI_DamageNum num = GetNumText();
        num.SetDamageNum((int)damage, damageType);
        
        num.worldPos = position.position + new Vector3(0,Random.Range(10,11),0);
        num.gameObject.SetActive(true);
    }

    private UI_DamageNum GetNumText()
    {
        int i = index++;
        if (index == Texts.Length) index = 0;
        return Texts[i];
    }
}
