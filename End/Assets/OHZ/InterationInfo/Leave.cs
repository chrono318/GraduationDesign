using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leave : MonoBehaviour
{
    void Update()
    {
        if (gameObject.GetComponent<InteractionObject>().canSetActiveUI && gameObject.GetComponent<InteractionObject>().isPressInterationButton)
        {
            SceneManager.LoadScene(3);
        }
    }
}
