using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FindAudioSourceInScene
{
    [MenuItem("Tools/Find AudioSource In Scene")]
    static void FindAudioSource()
    {
        Scene curScene = SceneManager.GetActiveScene();
        foreach(GameObject go in curScene.GetRootGameObjects())
        {
            CheckAudioSourceInGameObject(go);
        }
    }
    static bool CheckAudioSourceInGameObject(GameObject go)
    {
        if (go.GetComponent<AudioSource>())
        {
            Debug.Log(go.name);
            return true;
        }
        else
        {
            bool b = false;
            for(int i = 0; i < go.transform.childCount; i++)
            {
                b = b || CheckAudioSourceInGameObject(go.transform.GetChild(i).gameObject);
            }
            return b;
        }
    }
}
