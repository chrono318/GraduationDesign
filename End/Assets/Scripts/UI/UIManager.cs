using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UI_Bullet UI_Bullet;
    public Text E;
    public Slider San;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowE(bool bo,Transform tr)
    {
        E.transform.position = Camera.main.WorldToScreenPoint(tr.position + new Vector3(0, 15, 0));
        E.gameObject.SetActive(bo);
    }
    public void SetSanUI(float para)
    {
        San.value = para;
    }
}
