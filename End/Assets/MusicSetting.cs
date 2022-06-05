using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicSetting : MonoBehaviour
{
    bool mainMusic = true;
    bool normalAudio = true;
    public void CloseSetting()
    {
        gameObject.SetActive(false);
    }

    public GameObject MainMusicX;
    public GameObject NormalAudioX;
    public Button MainBtn;
    public Button SfxBtn;
    public Sprite opentex;
    public Sprite closetex;
    public void MainMusic()
    {
        if (mainMusic) //¹Ø±Õ
        {
            mainMusic = false;
            MainMusicX.SetActive(true);
            MainBtn.GetComponent<Image>().sprite = opentex;
            MainBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Open";
        }
        else
        {
            mainMusic = true;
            MainMusicX.SetActive(false);
            MainBtn.GetComponent<Image>().sprite = closetex;
            MainBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        }
        MusicCamera.mainMusic = mainMusic;
        MusicCamera.instance.UpdateMusicState();
    }
    public void NormalAudio()
    {
        if (normalAudio)
        {
            normalAudio = false;
            NormalAudioX.SetActive(true);
            SfxBtn.GetComponent<Image>().sprite = opentex;
            SfxBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Open";
        }
        else
        {
            normalAudio = true;
            NormalAudioX.SetActive(false);
            SfxBtn.GetComponent<Image>().sprite = closetex;
            SfxBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        }
        MusicCamera.normalAudio = normalAudio;
        MusicCamera.instance.UpdateMusicState();

    }
}
