using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackLoadScence : MonoBehaviour
{
    public string scenceName;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(scenceName);
        }
    }
}
