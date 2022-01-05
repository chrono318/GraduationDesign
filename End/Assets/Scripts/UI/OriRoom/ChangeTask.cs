using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTask : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        text.text = "新任务描述新任务描述新任务描述新任务描述新任务描述新任务描述新任务描述新任务描述新任务描述";
    }
}
