using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongFeng : MonoBehaviour
{
    public GameObject tips;
    public Material treemat;
    float a = 0.8f;
    private void Start()
    {
        treemat.SetFloat("_Height", a);
    }
    int i = 0;
    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<InteractionObject>().canSetActiveUI )
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                a += 0.032f;
                treemat.SetFloat("_Height", a);
                i++;
                if (i == 5)
                {
                    GameObject g = GameObject.Instantiate(tips);
                    g.GetComponent<UITips>().ShowTips("信物 + 10");
                }
            }
        }
    }
}
