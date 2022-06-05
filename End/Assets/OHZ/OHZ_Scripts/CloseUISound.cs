using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseUISound : MonoBehaviour
{
    
    public void PlayCloseSound()
    {
        SoundManager.instance.PlaySoundClip(SoundManager.instance.UISound[0]);
    }
    public void Back()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("LoadScene");
        Time.timeScale = 1f;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
