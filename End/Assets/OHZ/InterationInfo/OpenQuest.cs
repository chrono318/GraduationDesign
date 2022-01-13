using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenQuest : MonoBehaviour
{
    public GameObject quest;

    void Update()
    {
        if(gameObject.GetComponent<InteractionObject>().canSetActiveUI && gameObject.GetComponent<InteractionObject>().isPressInterationButton)
            quest.SetActive(true);
    }
}
