using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicCamera : MonoBehaviour
{
    public static bool mainMusic = true;
    public static bool normalAudio = true;
    public static MusicCamera instance;
    private void Awake()
    {   
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void OnEnable()
    {
        UpdateMusicState();
    }

    public void UpdateMusicState()
    {

        GetComponent<AudioSource>().enabled = mainMusic;

        //Scene curScene = SceneManager.GetActiveScene();
        //foreach (GameObject go in curScene.GetRootGameObjects())
        //{
        //    CloseAudioSourceInGameObject(go,normalAudio);
        //}
        GetComponent<AudioListener>().enabled = normalAudio;
    }
    void CloseAudioSourceInGameObject(GameObject go,bool state)
    {
        if (go.GetComponent<AudioSource>())
        {
            if (go.CompareTag("MainCamera"))
            {
                ;
            }
            else
            {
                go.GetComponent<AudioSource>().enabled = false;
            }
        }

        int childcount = go.transform.childCount;
        if(childcount == 0)
        {
            return;
        }
        else
        {
            for(int i = 0; i < childcount; i++)
            {
                CloseAudioSourceInGameObject(go.transform.GetChild(i).gameObject,state);
            }
        }
    }

    bool ispause = false;
    public GameObject pauseUI;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispause)
            {
                ispause = false;
                pauseUI.SetActive(false);
                Time.timeScale = 1f;
            }
            else //´ò¿ª
            {
                ispause = true;
                pauseUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        
    }
}
