using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiXinwu : MonoBehaviour
{
    public GameObject gameObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
