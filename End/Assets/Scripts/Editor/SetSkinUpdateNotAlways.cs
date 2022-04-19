using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

public class SetSkinUpdateNotAlways
{
    [MenuItem("Tools/Set Skin UpdateAlways False")]
    static void SetSkinUpdate()
    {
        Scene curScene = SceneManager.GetActiveScene();
        foreach (GameObject go in curScene.GetRootGameObjects())
        {
            SetSkinUpdateNotAlwaysIngo(go);
        }
        EditorUtility.DisplayDialog("修改spriteSkin", "Done", "Done");
    }
    static void SetSkinUpdateNotAlwaysIngo(GameObject go)
    {
        if (go.GetComponent<SpriteSkin>())
        {
            go.GetComponent<SpriteSkin>().alwaysUpdate = false;
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                SetSkinUpdateNotAlwaysIngo(go.transform.GetChild(i).gameObject);
            }
        }
    }
}
