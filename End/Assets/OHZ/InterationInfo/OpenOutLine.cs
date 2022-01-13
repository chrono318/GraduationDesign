using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOutLine : MonoBehaviour
{
    public GameObject outLine;

    void Update()
    {
            outLine.SetActive(gameObject.GetComponent<InteractionObject>().canSetActiveUI);
    }
}
